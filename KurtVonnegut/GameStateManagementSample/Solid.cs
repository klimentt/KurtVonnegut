using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagementSample
{
    public class Solid
    {
        public Texture2D Texture;

        public Vector2 Position;

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

        public void Initialize(Texture2D texture, Vector2 position)
        {
            this.Texture = texture;
            this.Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }
    }
}
