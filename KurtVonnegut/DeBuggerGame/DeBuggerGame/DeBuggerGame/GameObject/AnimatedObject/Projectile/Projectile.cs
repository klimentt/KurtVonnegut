namespace DeBuggerGame
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Projectile
        : GameObject
    {
        // image representing projectile
        public Texture2D Texture;

        // projectile position 
        public Vector2 Position;

        // state
        public bool Active;
                
        public int Damage;

        // represents viewable game boundary 
        Viewport viewport;

        // get width 
        public int Width
        {
            get { return Texture.Width; }
        }

        // get height 
        public int Height
        {
            get { return Texture.Height; }
        }
        

        float projectileMoveSpeed;


        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
        {
            this.Texture = texture;
            this.Position = position;
            this.viewport = viewport;

            this.Active = true;

            this.Damage = 4;

            this.projectileMoveSpeed = 9.0f;            
        }

        public void Update()
        {
            // Projectiles always move to the right
            this.Position.X += this.projectileMoveSpeed;

            // Deactivate the bullet if it goes out of screen
            if (this.Position.X + this.Texture.Width / 2 > this.viewport.Width)
            {
                this.Active = false;
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        spriteBatch.Draw(Texture, Position, null, Game.colors[Game.random.Next(4,7)], 0f,
        new Vector2(Width / 2, Height / 2), 1.5f, SpriteEffects.None, 0f);
    }
    }
}
