namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class FireAnt
        : EnemyAgent
    {
        public FireAnt()
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
            this.Speed = 0.8f;
        }


    }
}
