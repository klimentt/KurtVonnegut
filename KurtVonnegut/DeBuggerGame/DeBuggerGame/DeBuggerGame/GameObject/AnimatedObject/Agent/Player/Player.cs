namespace DeBuggerGame
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Player
        : Agent
    {
        #region constructors

        public Player()
            : base()
        {
            this.Health = 8;            
        }

        #endregion

        
    }
}
