namespace DeBugger
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class AnimatedObject
        : GameObject, IAnimateable, IGameObject
    {
        #region fields

        protected Animation animation;
        protected Vector2 position;

        #endregion



        #region properties

        public int Width
        {
            get { return this.animation.FrameWidth; }
        }

        public int Height
        {
            get { return this.animation.FrameHeight; }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public float X
        {
            get { return this.position.X; }
            set { this.position.X = value; }
        }

        public float Y
        {
            get { return this.position.Y; }
            set { this.position.Y = value; }
        }

        public bool Active { get; set; }

        #endregion



        #region constructors

        public AnimatedObject()
            : base()
        {
        }

        #endregion



        #region methods

        public virtual void Initialize(Animation animation, Vector2 position)
        {
            this.animation = animation;
            this.Position = position;
            this.Active = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            this.animation.Position = this.Position;
            this.animation.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.animation.Draw(spriteBatch);
        }

        #endregion
    }
}
