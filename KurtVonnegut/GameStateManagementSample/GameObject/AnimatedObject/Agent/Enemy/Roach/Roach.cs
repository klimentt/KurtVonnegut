namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Roach
        : RotatingEnemy
    {
        public Roach()
            : base()
        {
            this.Value = 2;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 8;

            // set enemy damage
            this.Damage = 10;

            // set enemy speed
            this.EnemyMoveSpeed = 2.0f;
        }


    }
}
