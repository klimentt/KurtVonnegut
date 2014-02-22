#region File Description

//-----------------------------------------------------------------------------
// MessageBoxScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

using System;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace GameStateManagementSample
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    internal class MessageBoxScreen : GameScreen
    {
        #region Fields
        
        private readonly string message;
        private Texture2D gradientTexture;
        
        private readonly InputAction menuSelect;
        private readonly InputAction menuCancel;

        #endregion
        
        #region Events
        
        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion
        
        #region Initialization
        
        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message) : this(message, true)
        {
        }
        
        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message, bool includeUsageText)
        {
            const string usageText = "\nA button, Space, Enter = ok" +
"\nB button, Esc = cancel"; 
            
            if (includeUsageText)
            {
                this.message = string.Format("{0}{1}", message, usageText);
            }
            else
            {
                this.message = message;
            }
            
            this.IsPopup = true;
            
            this.TransitionOnTime = TimeSpan.FromSeconds(0.2);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.2);
            
            this.menuSelect = new InputAction(
                new Buttons[] { Buttons.A, Buttons.Start },
                new Keys[] { Keys.Space, Keys.Enter },
                true);
            this.menuCancel = new InputAction(
                new Buttons[] { Buttons.B, Buttons.Back },
                new Keys[] { Keys.Escape, Keys.Back },
                true);
        }
        
        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                ContentManager content = this.ScreenManager.Game.Content;
                this.gradientTexture = content.Load<Texture2D>("gradient");
            }
        }

        #endregion
        
        #region Handle Input
        
        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;
            
            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (this.menuSelect.Evaluate(input, this.ControllingPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (this.Accepted != null)
                {
                    this.Accepted(this, new PlayerIndexEventArgs(playerIndex));
                }
                
                this.ExitScreen();
            }
            else if (this.menuCancel.Evaluate(input, this.ControllingPlayer, out playerIndex))
            {
                // Raise the cancelled event, then exit the message box.
                if (this.Cancelled != null)
                {
                    this.Cancelled(this, new PlayerIndexEventArgs(playerIndex));
                }
                
                this.ExitScreen();
            }
        }

        #endregion
        
        #region Draw
        
        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;
            SpriteFont font = this.ScreenManager.Font;
            
            // Darken down any other screens that were drawn beneath the popup.
            this.ScreenManager.FadeBackBufferToBlack(this.TransitionAlpha * 2 / 3);
            
            // Center the message text in the viewport.
            Viewport viewport = this.ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(this.message);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            
            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;
            
            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                (int)textPosition.Y - vPad,
                (int)textSize.X + hPad * 2,
                (int)textSize.Y + vPad * 2);
            
            // Fade the popup alpha during transitions.
            Color color = Color.White * this.TransitionAlpha;
            
            spriteBatch.Begin();
            
            // Draw the background rectangle.
            spriteBatch.Draw(this.gradientTexture, backgroundRectangle, color);
            
            // Draw the message box text.
            spriteBatch.DrawString(font, this.message, textPosition, color);
            
            spriteBatch.End();
        }

        #endregion
    }
}