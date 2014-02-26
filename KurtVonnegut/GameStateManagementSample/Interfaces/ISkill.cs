namespace DeBuggerGame
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

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
