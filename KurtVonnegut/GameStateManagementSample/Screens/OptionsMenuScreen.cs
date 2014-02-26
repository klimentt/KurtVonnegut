#region File Description

//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
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
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    internal class OptionsMenuScreen : MenuScreen
    {
        #region Fields
        
        private readonly MenuEntry ungulateMenuEntry;
        private readonly MenuEntry languageMenuEntry;
        private readonly MenuEntry frobnicateMenuEntry;
        private readonly MenuEntry elfMenuEntry;
        
        private enum Ungulate
        {
            BactrianCamel,
            Dromedary,
            Llama,
        }
        
        private static Ungulate currentUngulate = Ungulate.Dromedary;
        
        private static readonly string[] languages = { "C#", "French", "Deoxyribonucleic acid" };
        private static int currentLanguage = 0;
        
        private static bool frobnicate = true;
        
        private static int elf = 23;
        
        #endregion
        
        #region Initialization
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen() : base("Options")
        {
            // Create our menu entries.
            this.ungulateMenuEntry = new MenuEntry(string.Empty);
            this.languageMenuEntry = new MenuEntry(string.Empty);
            this.frobnicateMenuEntry = new MenuEntry(string.Empty);
            this.elfMenuEntry = new MenuEntry(string.Empty);
            
            this.SetMenuEntryText();
            
            MenuEntry back = new MenuEntry("Back");
            
            // Hook up menu event handlers.
            this.ungulateMenuEntry.Selected += this.UngulateMenuEntrySelected;
            this.languageMenuEntry.Selected += this.LanguageMenuEntrySelected;
            this.frobnicateMenuEntry.Selected += this.FrobnicateMenuEntrySelected;
            this.elfMenuEntry.Selected += this.ElfMenuEntrySelected;
            back.Selected += this.OnCancel;
            
            // Add entries to the menu.
            this.MenuEntries.Add(this.ungulateMenuEntry);
            this.MenuEntries.Add(this.languageMenuEntry);
            this.MenuEntries.Add(this.frobnicateMenuEntry);
            this.MenuEntries.Add(this.elfMenuEntry);
            this.MenuEntries.Add(back);
        }
        
        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        private void SetMenuEntryText()
        {
            this.ungulateMenuEntry.Text = string.Format("Preferred ungulate: {0}", currentUngulate);
            this.languageMenuEntry.Text = string.Format("Language: {0}", languages[currentLanguage]);
            this.frobnicateMenuEntry.Text = string.Format("Frobnicate: {0}", frobnicate ? "on" : "off");
            this.elfMenuEntry.Text = string.Format("elf: {0}", elf);
        }
        
        #endregion
        
        #region Handle Input
        
        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        private void UngulateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentUngulate++;
            
            if (currentUngulate > Ungulate.Llama)
            {
                currentUngulate = 0;
            }
            
            this.SetMenuEntryText();
        }
        
        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        private void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;
            
            this.SetMenuEntryText();
        }
        
        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        private void FrobnicateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            frobnicate = !frobnicate;
            
            this.SetMenuEntryText();
        }
        
        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        private void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            elf++;
            
            this.SetMenuEntryText();
        }
        
        #endregion
    }
}