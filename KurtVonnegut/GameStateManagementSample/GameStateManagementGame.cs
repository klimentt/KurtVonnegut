
#region File Description

//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

using System;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagementSample
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class GameStateManagementGame : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly ScreenManager screenManager;
        private readonly ScreenFactory screenFactory;

        SpriteBatch spriteBatch;
        
        /// <summary>
        /// The main game constructor.
        /// </summary>
        public GameStateManagementGame()
        {
            this.Content.RootDirectory = "Content";
            
            this.IsMouseVisible = true;
            this.graphics = new GraphicsDeviceManager(this);
            this.TargetElapsedTime = TimeSpan.FromTicks(333333);
            this.Window.Title = "DeBugger by Team Kurt Vonnegut"; // Add Window Title

            #if WINDOWS_PHONE
            graphics.IsFullScreen = true;
            
            // Choose whether you want a landscape or portait game by using one of the two helper functions.
            InitializeLandscapeGraphics();
            // InitializePortraitGraphics();
            #endif
            
            // Create the screen factory and add it to the Services
            this.screenFactory = new ScreenFactory();
            this.Services.AddService(typeof(IScreenFactory), this.screenFactory);
            
            // Create the screen manager component.
            this.screenManager = new ScreenManager(this);
            this.Components.Add(this.screenManager);
            
            #if WINDOWS_PHONE
            // Hook events on the PhoneApplicationService so we're notified of the application's life cycle
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Launching += 
                new EventHandler<Microsoft.Phone.Shell.LaunchingEventArgs>(GameLaunching);
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Activated += 
                new EventHandler<Microsoft.Phone.Shell.ActivatedEventArgs>(GameActivated);
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Deactivated += 
                new EventHandler<Microsoft.Phone.Shell.DeactivatedEventArgs>(GameDeactivated);
            #else
            // On Windows and Xbox we just add the initial screens
            this.AddInitialScreens();
            #endif
        }
        
        private void AddInitialScreens()
        {
            // Activate the first screens.
            this.screenManager.AddScreen(new BackgroundScreen(), null);
            
            // We have different menus for Windows Phone to take advantage of the touch interface
            #if WINDOWS_PHONE
            screenManager.AddScreen(new PhoneMainMenuScreen(), null);
            #else
            this.screenManager.AddScreen(new MainMenuScreen(), null);
            #endif
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            this.graphics.GraphicsDevice.Clear(Color.Black);
            
            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }

     
        #if WINDOWS_PHONE
        /// <summary>
        /// Helper method to the initialize the game to be a portrait game.
        /// </summary>
        private void InitializePortraitGraphics()
        {
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
        }
        /// <summary>
        /// Helper method to initialize the game to be a landscape game.
        /// </summary>
        private void InitializeLandscapeGraphics()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }
        void GameLaunching(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            AddInitialScreens();
        }
        void GameActivated(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            // Try to deserialize the screen manager
            if (!screenManager.Activate(e.IsApplicationInstancePreserved))
            {
                // If the screen manager fails to deserialize, add the initial screens
                AddInitialScreens();
            }
        }
        void GameDeactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            // Serialize the screen manager when the game deactivated
            screenManager.Deactivate();
        }
        #endif
    }
}