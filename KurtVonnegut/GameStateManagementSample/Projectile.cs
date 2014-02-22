using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagementSample
{
    public class Projectile : IRotatable, IGameObject
    {
        

        // Image representing the Projectile
        public Texture2D Texture;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position { get; set; }

        // State of the Projectile
        public bool Active;

        // The amount of damage the projectile can inflict to an enemy
        public int Damage;

        private const int DAMAGE = 15;
        private const float SPEED = 17;

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


            this.Active = true;

            this.Damage = DAMAGE;

            this.projectileMoveSpeed = SPEED;
        }

        public void Update()
        {
            // can move sideways by rotation
            float offsetX = (float)Math.Cos(this.Rotation);
            float offsetY = (float)Math.Sin(this.Rotation);
            this.Position += new Vector2(this.projectileMoveSpeed * offsetX, this.projectileMoveSpeed * offsetY);

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



        public void RotateTowards(Vector2 position)
        {
            throw new NotImplementedException();
        }
    }
}
