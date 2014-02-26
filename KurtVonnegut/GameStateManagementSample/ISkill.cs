using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    public interface ISkill : IGameObject, IFireble
    {
        Animation Animation { get; set; }
        Vector2 StartPosition { get; set; }
        void Activate(GameTime time);
        void Initialize(Vector2 startPosition, Animation animation, TimeSpan cooldown);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
