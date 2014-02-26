namespace DeBugger
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MonitorWep
        : Weapon
    {
        #region constructor

        public MonitorWep()
            : base()
        {
            this.Active = true;
            this.Value = 5;
            this.Damage = 5;
        }

        #endregion
    }
}