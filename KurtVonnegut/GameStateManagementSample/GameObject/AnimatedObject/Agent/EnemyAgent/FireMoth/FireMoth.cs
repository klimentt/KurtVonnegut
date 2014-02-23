namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class FireMoth
        : EnemyAgent
    {
        public FireMoth()
            : base()
        {
            this.Value = 3;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 8;

            // set enemy damage
            this.Damage = 3;

            // set enemy speed
            this.Speed = 0.3f;
        }


    }
}
