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

namespace DeBugger.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    public class CharacterSelectionScreen : MenuScreen
    {
        #region Fields

        private readonly MenuEntry raceMenuEntry;
        private readonly MenuEntry weaponMenuEntry;
        private readonly MenuEntry genderMenuEntry;
        //TODO: Move to a separate file
        public enum Race
        {
            Programmer,
            Designer,
            Administrator,
        }

        public static Race currentRace = Race.Programmer;

        private static readonly string[] weapons = { "C#", "JavaScript", "Python" };
        private static int currentWeapon = 0;

        private static bool gender = true;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public CharacterSelectionScreen()
            : base("Hero:")
        {
            // Create our menu entries.
            this.raceMenuEntry = new MenuEntry(string.Empty);
            this.weaponMenuEntry = new MenuEntry(string.Empty);
            this.genderMenuEntry = new MenuEntry(string.Empty);

            this.SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            this.raceMenuEntry.Selected += this.RaceMenuEntrySelected;
            this.weaponMenuEntry.Selected += this.WeaponMenuEntrySelected;
            this.genderMenuEntry.Selected += this.GenderMenuEntrySelected;
            back.Selected += this.OnCancel;

            // Add entries to the menu.
            this.MenuEntries.Add(this.raceMenuEntry);
            this.MenuEntries.Add(this.weaponMenuEntry);
            this.MenuEntries.Add(this.genderMenuEntry);
            this.MenuEntries.Add(back);
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        private void SetMenuEntryText()
        {
            this.raceMenuEntry.Text = string.Format("<{0}>", currentRace);
            this.weaponMenuEntry.Text = string.Format("Weapon: {0}", weapons[currentWeapon]);
            this.genderMenuEntry.Text = string.Format("Gender: {0}", gender ? "Male" : "Female");
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        private void RaceMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentRace++;

            if (currentRace > Race.Administrator)
            {
                currentRace = 0;
            }

            this.SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        private void WeaponMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentWeapon = (currentWeapon + 1) % weapons.Length;

            this.SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        private void GenderMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            gender = !gender;

            this.SetMenuEntryText();
        }

        #endregion
    }
}