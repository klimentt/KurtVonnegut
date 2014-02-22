#region File Description

//-----------------------------------------------------------------------------
// ScreenManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The screen manager is a component which manages one or more GameScreen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields
        
        private const string StateFilename = "ScreenManagerState.xml";
        
        private readonly List<GameScreen> screens = new List<GameScreen>();
        private readonly List<GameScreen> tempScreensList = new List<GameScreen>();
        
        private readonly InputState input = new InputState();
        
        private bool isInitialized;
        
        // Required for music
        private SoundEffect backgroundMusic;
        
        //test
        
        #endregion
        
        #region Properties
        
        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }
        
        /// <summary>
        /// A default font shared by all the screens. This saves
        /// each screen having to bother loading their own local copy.
        /// </summary>
        public SpriteFont Font { get; private set; }
        
        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled { get; set; }
        
        /// <summary>
        /// Gets a blank texture that can be used by the screens.
        /// </summary>
        public Texture2D BlankTexture { get; private set; }
        
        #endregion
        
        #region Initialization
        
        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(Game game) : base(game)
        {
            // we must set EnabledGestures before we can query for them, but
            // we don't assume the game wants to read them.
            TouchPanel.EnabledGestures = GestureType.None;
        }
        
        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            
            this.isInitialized = true;
        }
        
        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            ContentManager content = this.Game.Content;
            
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Font = content.Load<SpriteFont>("menufont");
            this.BlankTexture = content.Load<Texture2D>("blank");
            
            // Load Background Music
            this.backgroundMusic = content.Load<SoundEffect>("Sounds\\backgroundMusic");
            this.backgroundMusic.Play();
            
            // Tell each of the screens to load their content.
            foreach (GameScreen screen in this.screens)
            {
                screen.Activate(false);
            }
        }
        
        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in this.screens)
            {
                screen.Unload();
            }
        }
        
        #endregion
        
        #region Update and Draw
        
        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            this.input.Update();
            
            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            this.tempScreensList.Clear();
            
            foreach (GameScreen screen in this.screens)
            {
                this.tempScreensList.Add(screen);
            }
            
            bool otherScreenHasFocus = !this.Game.IsActive;
            bool coveredByOtherScreen = false;
            
            // Loop as long as there are screens waiting to be updated.
            while (this.tempScreensList.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = this.tempScreensList[this.tempScreensList.Count - 1];
                
                this.tempScreensList.RemoveAt(this.tempScreensList.Count - 1);
                
                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
                
                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(gameTime, this.input);
                        
                        otherScreenHasFocus = true;
                    }
                    
                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                    {
                        coveredByOtherScreen = true;
                    }
                }
            }
            
            // Print debug trace?
            if (this.TraceEnabled)
            {
                this.TraceScreens();
            }
        }
        
        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        private void TraceScreens()
        {
            List<string> screenNames = new List<string>();
            
            foreach (GameScreen screen in this.screens)
            {
                screenNames.Add(screen.GetType().Name);
            }
            
            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }
        
        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in this.screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                {
                    continue;
                }
                
                screen.Draw(gameTime);
            }
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;
            
            // If we have a graphics device, tell the screen to load content.
            if (this.isInitialized)
            {
                screen.Activate(false);
            }
            
            this.screens.Add(screen);
            
            // update the TouchPanel to respond to gestures this screen is interested in
            TouchPanel.EnabledGestures = screen.EnabledGestures;
        }
        
        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (this.isInitialized)
            {
                screen.Unload();
            }
            
            this.screens.Remove(screen);
            this.tempScreensList.Remove(screen);
            
            // if there is a screen still in the manager, update TouchPanel
            // to respond to gestures that screen is interested in.
            if (this.screens.Count > 0)
            {
                TouchPanel.EnabledGestures = this.screens[this.screens.Count - 1].EnabledGestures;
            }
        }
        
        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public GameScreen[] GetScreens()
        {
            return this.screens.ToArray();
        }
        
        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups.
        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            this.SpriteBatch.Begin();
            this.SpriteBatch.Draw(this.BlankTexture, this.GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            this.SpriteBatch.End();
        }
        
        /// <summary>
        /// Informs the screen manager to serialize its state to disk.
        /// </summary>
        public void Deactivate()
        {
            #if !WINDOWS_PHONE
            return;
            #else
// Open up isolated storage
using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
{
// Create an XML document to hold the list of screen types currently in the stack
XDocument doc = new XDocument();
XElement root = new XElement("ScreenManager");
doc.Add(root);
// Make a copy of the master screen list, to avoid confusion if
// the process of deactivating one screen adds or removes others.
tempScreensList.Clear();
foreach (GameScreen screen in screens)
tempScreensList.Add(screen);
// Iterate the screens to store in our XML file and deactivate them
foreach (GameScreen screen in tempScreensList)
{
// Only add the screen to our XML if it is serializable
if (screen.IsSerializable)
{
// We store the screen's controlling player so we can rehydrate that value
string playerValue = screen.ControllingPlayer.HasValue
? screen.ControllingPlayer.Value.ToString()
: "";
root.Add(new XElement(
"GameScreen",
new XAttribute("Type", screen.GetType().AssemblyQualifiedName),
new XAttribute("ControllingPlayer", playerValue)));
}
// Deactivate the screen regardless of whether we serialized it
screen.Deactivate();
}
// Save the document
using (IsolatedStorageFileStream stream = storage.CreateFile(StateFilename))
{
doc.Save(stream);
                }
}
            #endif
        }
        
        public bool Activate(bool instancePreserved)
        {
            #if !WINDOWS_PHONE
            return false;
            #else
// If the game instance was preserved, the game wasn't dehydrated so our screens still exist.
// We just need to activate them and we're ready to go.
if (instancePreserved)
{
// Make a copy of the master screen list, to avoid confusion if
// the process of activating one screen adds or removes others.
tempScreensList.Clear();
foreach (GameScreen screen in screens)
tempScreensList.Add(screen);
foreach (GameScreen screen in tempScreensList)
screen.Activate(true);
}
// Otherwise we need to refer to our saved file and reconstruct the screens that were present
// when the game was deactivated.
else
{
// Try to get the screen factory from the services, which is required to recreate the screens
IScreenFactory screenFactory = Game.Services.GetService(typeof(IScreenFactory)) as IScreenFactory;
if (screenFactory == null)
{
throw new InvalidOperationException(
"Game.Services must contain an IScreenFactory in order to activate the ScreenManager.");
}
// Open up isolated storage
using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
{
// Check for the file; if it doesn't exist we can't restore state
if (!storage.FileExists(StateFilename))
return false;
// Read the state file so we can build up our screens
using (IsolatedStorageFileStream stream = storage.OpenFile(StateFilename, FileMode.Open))
{
XDocument doc = XDocument.Load(stream);
// Iterate the document to recreate the screen stack
foreach (XElement screenElem in doc.Root.Elements("GameScreen"))
{
// Use the factory to create the screen
Type screenType = Type.GetType(screenElem.Attribute("Type").Value);
GameScreen screen = screenFactory.CreateScreen(screenType);
// Rehydrate the controlling player for the screen
PlayerIndex? controllingPlayer = screenElem.Attribute("ControllingPlayer").Value != ""
? (PlayerIndex)Enum.Parse(typeof(PlayerIndex), screenElem.Attribute("ControllingPlayer").Value, true)
: (PlayerIndex?)null;
screen.ControllingPlayer = controllingPlayer;
// Add the screen to the screens list and activate the screen
screen.ScreenManager = this;
screens.Add(screen);
screen.Activate(false);
// update the TouchPanel to respond to gestures this screen is interested in
TouchPanel.EnabledGestures = screen.EnabledGestures;
}
}
}
}
return true;
            #endif
        }
        #endregion
    }
}