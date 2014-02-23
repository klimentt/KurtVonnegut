namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Banana
        : Consummable
    {
        #region fields



        #endregion

        #region properties


        #endregion

        #region constructor

        public Banana()
            : base()
        {
            this.Active = true;
            this.Value = 1;
            this.Heal = 3;
        }


        #endregion
    }
}
