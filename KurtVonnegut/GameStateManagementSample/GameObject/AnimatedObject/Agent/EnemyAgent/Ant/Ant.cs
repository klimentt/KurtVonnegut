namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Ant
        : EnemyAgent
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
            this.Damage = 1;

            // set enemy speed
            this.Speed = 0.4f;
        }


    }
}
