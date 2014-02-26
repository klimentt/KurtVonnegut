using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeBuggerGame
{
    public class Enemy : Agent, IGameObject
    {

        protected const float DEF_SPEED = 2.0f;
        protected const int HEALTH = 10;
        protected const int DAMAGE = 50;
        protected const int XP_VALUE = 50;

        

        // The hit points of the enemy, if this goes to zero the enemy dies
        public int Health;
        
        // The amount of damage the enemy inflicts on the player ship
        public int Damage;

        // The amount of score the enemy will give to the player
        public int Value;

        

        // The speed at which the enemy moves
        protected virtual float EnemyMoveSpeed { get; set; }

        

        public override void Initialize(Animation animation, Vector2 position)
        {
            // Load the enemy ship texture
            this.animation = animation;

            // Set the position of the enemy
            this.Position = position;

            // We initialize the enemy to be active so it will be update in the game
            this.Active = true;

            // Set the health of the enemy
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
            // The enemy always moves to the left so decrement it's x position
            this.Position -= new Vector2(this.EnemyMoveSpeed, 0);

            // Update the position of the Animation
            this.animation.Position = this.Position;

            // Update Animation
            this.animation.Update(gameTime);

            // If the enemy is past the screen or its health reaches 0 then deactivate it
            if (this.Position.X < -this.Width || this.Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet from the
                // active game list
                this.Active = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            this.animation.Draw(spriteBatch);
        }
    }
}
