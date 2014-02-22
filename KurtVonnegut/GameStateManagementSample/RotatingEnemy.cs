using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public class RotatingEnemy : Enemy, IRotatable, IGameObject, IAggressive
    {
        protected new const float DEF_SPEED = 2.0f;
        protected new const int HEALTH = 10;
        protected new const int DAMAGE = 50;
        protected new const int XP_VALUE = 50;
        public float Rotation { get; set; }

        public bool IsInAggroRange { get; set; }

        public void Initialize(Animation animation, Vector2 position, float rotation)
        {
            this.Rotation = rotation;
            this.IsInAggroRange = false;
            base.Initialize(animation, position);
        }
        public override void Update(GameTime gameTime)
        {
            if (IsInAggroRange)
            {
                // can move sideways by rotation
                float offsetX = (float)Math.Cos(this.Rotation);
                float offsetY = (float)Math.Sin(this.Rotation);
                this.Position += new Vector2(this.EnemyMoveSpeed * offsetX, this.EnemyMoveSpeed * offsetY);   
            }
            

            // Update the position of the Animation
            this.EnemyAnimation.Position = this.Position;

            // Update Animation
            this.EnemyAnimation.Update(gameTime);

            // If  its health reaches 0 then deactivate it
            if (this.Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet from the
                // active game list
                this.Active = false;
            }
        }

        public void RotateTowards(Vector2 position)
        {
            float distanceX = this.Position.X - this.Width / 2 - position.X;
            float distanceY = this.Position.Y - this.Height / 2 - position.Y;
            this.Rotation = (float)Math.Atan2(distanceY, distanceX) - MathHelper.Pi;
        }
    }
}
