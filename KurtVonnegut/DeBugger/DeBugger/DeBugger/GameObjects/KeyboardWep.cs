namespace DeBugger
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class KeyboardWep
        : Weapon
    {
        #region constructor

        public KeyboardWep()
            : base()
        {
            this.Active = true;
            this.Value = 2;
            this.Damage = 3;
        }

        #endregion
    }
}