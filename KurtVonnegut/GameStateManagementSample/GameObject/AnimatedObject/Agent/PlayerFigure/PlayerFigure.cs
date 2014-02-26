namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class PlayerFigure
        : Agent
    {
        #region constructors

        public PlayerFigure()
            : base()
        {
            this.Health = 8;
        }

        #endregion


    }
}
