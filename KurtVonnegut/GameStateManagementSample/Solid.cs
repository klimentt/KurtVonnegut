using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeBuggerGame
{
    public class Solid : IGameObject
    {
        protected float Scale {get ; private set;}
        public Texture2D Texture;

        public Vector2 Position { get; set; }

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

        public virtual void Initialize(Texture2D texture, Vector2 position, float scale)
        {

            this.Scale = scale;
            this.Texture = texture;
            this.Position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRectangle =  new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);

            spriteBatch.Draw(this.Texture, destRectangle, null, Color.White, 0f, new Vector2(this.Width / 2, this.Height / 2), SpriteEffects.None, 0f);
        }
    }
}
