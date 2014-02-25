namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Ant
        : RotatingEnemy        
    {        
        public Ant()
            : base()
        {
            this.Value = 2;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 8;

            // set enemy damage
            this.Damage = 60;

            // set enemy speed
            this.EnemyMoveSpeed = 0.4f;
        }


    }
}
