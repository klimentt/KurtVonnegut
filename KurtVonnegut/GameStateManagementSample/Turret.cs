using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    public class Turret : Solid, IRotatable, IFireble, IGameObject
    {
        private const float FIRE_DELAY = 1.80f;
        // The rate of fire of the projectile laser
        public TimeSpan FireTime { get; set; }
        public TimeSpan PreviousFireTime { get; set; }
        public float Rotation { get; set; }

        public void Initialize(Texture2D texture, Microsoft.Xna.Framework.Vector2 position, float scale, float rotation)
        {
            this.Rotation = rotation;
            FireTime = TimeSpan.FromSeconds(FIRE_DELAY);
            base.Initialize(texture, position, scale);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRectangle = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);

            spriteBatch.Draw(this.Texture, destRectangle, null, Color.White, this.Rotation, new Vector2(this.Width/2, this.Height/2), SpriteEffects.None, 0f);
        }

        public void RotateTowards(Vector2 position)
        {
            float distanceX = this.Position.X - this.Width / 2 - position.X;
            float distanceY = this.Position.Y - this.Height / 2 - position.Y;
            this.Rotation = (float)Math.Atan2(distanceY, distanceX) - MathHelper.Pi;
        }
    }
}
