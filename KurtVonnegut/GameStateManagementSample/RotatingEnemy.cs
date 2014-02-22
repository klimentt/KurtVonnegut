using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public class RotatingEnemy : Enemy, IRotatable, IGameObject, IAggressive, IFireble
    {
        protected new const float DEF_SPEED = 5.0f;
        protected new const int HEALTH = 10;
        protected new const int DAMAGE = 5;
        protected new const int XP_VALUE = 50;
        protected const float ATTACK_SPEED = 1.2f;

        public TimeSpan FireTime { get; set; }
        public TimeSpan PreviousFireTime { get; set; }
        public float Rotation { get; set; }
        public float AggroRange { get; set; }

        public bool IsInAggroRange { get; set; }

        public void Initialize(Animation animation, Vector2 position, float rotation, float aggroRange)
        {
            this.FireTime = TimeSpan.FromSeconds(ATTACK_SPEED);
            this.PreviousFireTime = TimeSpan.FromSeconds(0);
            this.AggroRange = aggroRange;
            this.Rotation = rotation;
            this.IsInAggroRange = false;
            //call base, its important to be here as we are writing over properties below
            base.Initialize(animation, position);

            this.Health = HEALTH;
            // Set the amount of damage the enemy can do
            this.Damage = DAMAGE;

            // Set how fast the enemy moves
            this.EnemyMoveSpeed = DEF_SPEED;

            // Set the score value of the enemy
            this.Value = XP_VALUE;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.EnemyAnimation.Draw(spriteBatch, this);
        }
        public void RotateTowards(Vector2 position)
        {
            float distanceX = this.Position.X - this.Width / 2 - position.X;
            float distanceY = this.Position.Y - this.Height / 2 - position.Y;
            this.Rotation = (float)Math.Atan2(distanceY, distanceX) - MathHelper.Pi;
        }
    }
}
