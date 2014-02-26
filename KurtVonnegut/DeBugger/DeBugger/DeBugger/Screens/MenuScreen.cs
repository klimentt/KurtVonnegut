#region File Description

//-----------------------------------------------------------------------------
// MenuScreen.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace DeBugger
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    public abstract class MenuScreen : GameScreen
    {
        #region Fields

        private readonly List<MenuEntry> menuEntries = new List<MenuEntry>();
        private int selectedEntry = 0;
        private readonly string menuTitle;

        private readonly InputAction menuUp;
        private readonly InputAction menuDown;
        private readonly InputAction menuSelect;
        private readonly InputAction menuCancel;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        public IList<MenuEntry> MenuEntries
        {
            get
            {
                return this.menuEntries;
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            this.TransitionOnTime = TimeSpan.FromSeconds(0.5);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.menuUp = new InputAction(
                new Buttons[] { Buttons.DPadUp, Buttons.LeftThumbstickUp },
                new Keys[] { Keys.Up },
                true);
            this.menuDown = new InputAction(
                new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
                new Keys[] { Keys.Down },
                true);
            this.menuSelect = new InputAction(
                new Buttons[] { Buttons.A, Buttons.Start },
                new Keys[] { Keys.Enter, Keys.Space },
                true);
            this.menuCancel = new InputAction(
                new Buttons[] { Buttons.B, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            // For input tests we pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            // Move to the previous menu entry?
            if (this.menuUp.Evaluate(input, this.ControllingPlayer, out playerIndex))
            {
                this.selectedEntry--;

                if (this.selectedEntry < 0)
                {
                    this.selectedEntry = this.menuEntries.Count - 1;
                }
            }

            // Move to the next menu entry?
            if (this.menuDown.Evaluate(input, this.ControllingPlayer, out playerIndex))
            {
                this.selectedEntry++;

                if (this.selectedEntry >= this.menuEntries.Count)
                {
                    this.selectedEntry = 0;
                }
            }

            if (this.menuSelect.Evaluate(input, this.ControllingPlayer, out playerIndex))
            {
                this.OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (this.menuCancel.Evaluate(input, this.ControllingPlayer, out playerIndex))
            {
                this.OnCancel(playerIndex);
            }
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            this.menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            this.ExitScreen();
        }

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        public void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            this.OnCancel(e.PlayerIndex);
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(this.TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, 310f);

            // update each menu entry's location in turn
            for (int i = 0; i < this.menuEntries.Count; i++)
            {
                MenuEntry menuEntry = this.menuEntries[i];

                // each entry is to be centered horizontally
                position.X = this.ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (this.ScreenState == ScreenState.TransitionOn)
                {
                    position.X -= transitionOffset * 256;
                }
                else
                {
                    position.X += transitionOffset * 512;
                }

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);
            }
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < this.menuEntries.Count; i++)
            {
                bool isSelected = this.IsActive && (i == this.selectedEntry);

                this.menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            this.UpdateMenuEntryLocations();

            GraphicsDevice graphics = this.ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;
            SpriteFont font = this.ScreenManager.Font;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < this.menuEntries.Count; i++)
            {
                MenuEntry menuEntry = this.menuEntries[i];

                bool isSelected = this.IsActive && (i == this.selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(this.TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 130);
            Vector2 titleOrigin = font.MeasureString(this.menuTitle) / 2;
            Color titleColor = new Color(255, 255, 0) * this.TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, this.menuTitle, titlePosition, titleColor, 0,
                titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        #endregion
    }
}