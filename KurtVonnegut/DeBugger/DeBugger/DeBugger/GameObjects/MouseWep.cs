namespace DeBugger
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MouseWep
        : Weapon
    {
        #region constructor

        public MouseWep()
            : base()
        {
            this.Active = true;
            this.Value = 1;
            this.Damage = 2;
        }

        #endregion
    }
}