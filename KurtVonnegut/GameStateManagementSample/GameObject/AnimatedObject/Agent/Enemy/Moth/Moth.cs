namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Moth
        : RotatingEnemy
    {
        public Moth()
            : base()
        {
            this.Value = 3;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 8;

            // set enemy damage
            this.Damage = 1;

            // set enemy speed
            this.EnemyMoveSpeed = 0.6f;
        }


    }
}

