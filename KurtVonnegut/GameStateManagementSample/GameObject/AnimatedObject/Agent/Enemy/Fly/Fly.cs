namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Fly
        : RotatingEnemy
    {
        public Fly()
            : base()
        {
            this.Value = 2;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 12;

            // set enemy damage
            this.Damage = 5;

            // set enemy speed
            this.EnemyMoveSpeed = 1.5f;
        }


    }
}

