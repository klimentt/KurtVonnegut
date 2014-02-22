using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagementSample
{
    public class Enemy
    {
        // Animation representing the enemy
        public Animation EnemyAnimation;

        // The position of the enemy ship relative to the top left corner of thescreen
        public Vector2 Position;

        // The state of the Enemy Ship
        public bool Active;

        // The hit points of the enemy, if this goes to zero the enemy dies
        public int Health;
        
        // The amount of damage the enemy inflicts on the player ship
        public int Damage;

        // The amount of score the enemy will give to the player
        public int Value;

        private const float DEF_SPEED = 2.0f;
        private const int HEALTH = 10;
        private const int DAMAGE = 50;
        private const int SCORE_VALUE = 50;

        // The speed at which the enemy moves
        private float enemyMoveSpeed;

        // Get the width of the enemy ship
        public int Width
        {
            get
            {
                return this.EnemyAnimation.FrameWidth;
            }
        }

        // Get the height of the enemy ship
        public int Height
        {
            get
            {
                return this.EnemyAnimation.FrameHeight;
            }
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            // Load the enemy ship texture
            this.EnemyAnimation = animation;

            // Set the position of the enemy
            this.Position = position;

            // We initialize the enemy to be active so it will be update in the game
            this.Active = true;

            // Set the health of the enemy
            this.Health = HEALTH;

            // Set the amount of damage the enemy can do
            this.Damage = DAMAGE;

            // Set how fast the enemy moves
            this.enemyMoveSpeed = DEF_SPEED;

            // Set the score value of the enemy
            this.Value = SCORE_VALUE;
        }

        public void Update(GameTime gameTime)
        {
            // The enemy always moves to the left so decrement it's x position
            this.Position.X -= this.enemyMoveSpeed;

            // Update the position of the Animation
            this.EnemyAnimation.Position = this.Position;

            // Update Animation
            this.EnemyAnimation.Update(gameTime);

            // If the enemy is past the screen or its health reaches 0 then deactivate it
            if (this.Position.X < -this.Width || this.Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet from the
                // active game list
                this.Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            this.EnemyAnimation.Draw(spriteBatch);
        }
    }
}
