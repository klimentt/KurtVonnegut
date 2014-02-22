using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public class Turret : Solid, IRotatable
    {
        public float Rotation { get; set; }

        public void Initialize(Texture2D texture, Microsoft.Xna.Framework.Vector2 position, float scale, float rotation)
        {
            this.Rotation = rotation;
            base.Initialize(texture, position, scale);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);

            spriteBatch.Draw(this.Texture, destRectangle, null, Color.White, this.Rotation, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
