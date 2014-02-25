namespace GameStateManagementSample
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
            this.Value = 8;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 30;

            // set enemy damage
            this.Damage = 12;

            // set enemy speed
            this.EnemyMoveSpeed = 1.1f;
        }
    }
}

