namespace DeBuggerGame
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Coffee
        : Consummable
    {
        #region fields



        #endregion

        #region properties



        #endregion

        #region constructor

        public Coffee()
            : base()
        {
            this.Active = true;
            this.Value = 1;
            this.Heal = 5;
        }


        #endregion
    }
}
