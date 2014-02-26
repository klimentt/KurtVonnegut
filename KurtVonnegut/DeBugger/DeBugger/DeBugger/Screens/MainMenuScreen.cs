#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion
#region Using Statements
using DeBugger.Screens;
using Microsoft.Xna.Framework;

#endregion

namespace DeBugger
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    internal class MainMenuScreen : MenuScreen
    {
        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("DeBugger")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry characterMenuEntry = new MenuEntry("Character");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += this.PlayGameMenuEntrySelected;
            characterMenuEntry.Selected += this.CharacterMenuEntrySelected;
            exitMenuEntry.Selected += this.OnCancel;

            // Add entries to the menu.
            this.MenuEntries.Add(playGameMenuEntry);
            this.MenuEntries.Add(characterMenuEntry);
            this.MenuEntries.Add(exitMenuEntry);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(this.ScreenManager, true, e.PlayerIndex,
                new GameplayScreen());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        private void CharacterMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ScreenManager.AddScreen(new CharacterSelectionScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string MESSAGE = "Are you sure you want to exit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(MESSAGE);

            confirmExitMessageBox.Accepted += this.ConfirmExitMessageBoxAccepted;

            this.ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            this.ScreenManager.Game.Exit();
        }

        #endregion
    }
}