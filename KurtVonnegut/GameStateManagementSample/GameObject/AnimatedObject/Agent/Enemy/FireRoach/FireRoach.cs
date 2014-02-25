namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class FireRoach
        : RotatingEnemy
    {
        public FireRoach()
            : base()
        {
            this.Value = 6;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 16;

            // set enemy damage
            this.Damage = 15;

            // set enemy speed
            this.EnemyMoveSpeed = 1.0f;
        }


    }
}
