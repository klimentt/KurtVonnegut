using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagementSample
{
    public abstract class Skill : ISkill
    {
        public Skill()
        {
        }


        public Animation Animation {get; set; }

        public Vector2 StartPosition { get; set; }

        public Vector2 Position { get; set; }

        public int Width
        {
            get { return this.Animation.FrameWidth; }
        }

        public int Height
        {
            get { return this.Animation.FrameHeight; }
        }
        //used to keep cooldowns
        public TimeSpan FireTime { get; set; }

        public TimeSpan PreviousFireTime { get; set; }

        public abstract void Activate(GameTime time);
        public virtual void Initialize(Vector2 startPosition, Animation animation, TimeSpan cooldown)
        {
            this.Animation = animation;
            this.FireTime = cooldown;
            this.StartPosition = startPosition;
        }


        public virtual void Update(GameTime gameTime)
        {
            //TODO Update
            throw new NotImplementedException();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.Animation.Draw(spriteBatch);
        }
    }
}
