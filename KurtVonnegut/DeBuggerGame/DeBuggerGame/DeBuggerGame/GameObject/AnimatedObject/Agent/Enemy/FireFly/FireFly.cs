namespace DeBuggerGame
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class FireFly
        : Enemy
    {
        public FireFly()
            : base()
        {
            this.Value = 5;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 8;

            // set enemy damage
            this.Damage = 2;

            // set enemy speed
            this.Speed = 1.1f;
        }


    }
}
