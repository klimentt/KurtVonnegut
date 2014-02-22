
#region File Description

//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

#endregion

namespace GameStateManagementSample
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    internal class PauseMenuScreen : MenuScreen
    {
        #region Initialization
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen() : base("Paused")
        {
            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");
            
            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += this.OnCancel;
            quitGameMenuEntry.Selected += this.QuitGameMenuEntrySelected;

            // Add entries to the menu.
            this.MenuEntries.Add(resumeGameMenuEntry);
            this.MenuEntries.Add(quitGameMenuEntry);
        }
        
        #endregion
        
        #region Handle Input
        
        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += this.ConfirmQuitMessageBoxAccepted;
            
            this.ScreenManager.AddScreen(confirmQuitMessageBox, this.ControllingPlayer);
        }
        
        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(this.ScreenManager, false, null, new BackgroundScreen(),
                new MainMenuScreen());
        }

        #endregion
    }
}