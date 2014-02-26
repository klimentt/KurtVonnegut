namespace DeBugger
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class EnergyDrink
        : Consummable
    {
        #region fields



        #endregion

        #region properties



        #endregion

        #region constructor

        public EnergyDrink()
            : base()
        {
            this.Active = true;
            this.Value = 4;
            this.Heal = 8;
        }
        
        #endregion
    }
}
