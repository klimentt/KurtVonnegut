using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagementSample
{
    public class Projectile : IRotatable
    {
        // The rate of fire of the projectile laser
        public static TimeSpan fireTime;
        public static TimeSpan previousFireTime;

        // Image representing the Projectile
        public Texture2D Texture;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position;

        // State of the Projectile
        public bool Active;

        // The amount of damage the projectile can inflict to an enemy
        public int Damage;

        private const int DAMAGE = 15;
        private const float SPEED = 20;
        private const float FIRE_DELAY = 0.15f;

        // Represents the viewable boundary of the game
        private Viewport viewport;

        // Determines how fast the projectile moves
        private float projectileMoveSpeed;

        // Get the width of the projectile ship
        public int Width
        {
            get
            {
                return this.Texture.Width;
            }
        }

        // Get the height of the projectile ship
        public int Height
        {
            get
            {
                return this.Texture.Height;
            }
        }

        public float Rotation { get; set; }

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, IRotatable shooter)
        {
            this.Texture = texture;
            this.Position = position;
            this.viewport = viewport;
            this.Rotation = shooter.Rotation;
            // Set the laser to fire every quarter second
            fireTime = TimeSpan.FromSeconds(FIRE_DELAY);

            this.Active = true;

            this.Damage = DAMAGE;

            this.projectileMoveSpeed = SPEED;
        }

        public void Update()
        {
            // can move sideways by rotation
            float offsetX = (float)Math.Cos(this.Rotation);
            float offsetY = (float)Math.Sin(this.Rotation);
            this.Position.X += this.projectileMoveSpeed * offsetX;
            this.Position.Y += this.projectileMoveSpeed * offsetY;

            // Deactivate the bullet if it goes out of screen
            if (this.Position.X + this.Texture.Width / 2 > this.viewport.Width)
            {
                this.Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, null, Color.White, this.Rotation, new Vector2(this.Width / 2, this.Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}