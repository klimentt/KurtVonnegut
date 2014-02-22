namespace DeBuggerGame
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Agent
        : AnimatedObject
    {

        #region fields

        protected int health;

        #endregion

        #region properties

        public int Health
        {
            get
            {
                if (this.health <= 0)
                {
                    throw new System.ApplicationException("Game over");
                }
                return this.health;
            }
            set
            {
                if (value < 0 || value > 8)
                {
                    throw new System.ArgumentOutOfRangeException("Health must be in the range [0;8]!");
                }
                this.health = value;
            }
        }

        

        #endregion

        #region constructors

        public Agent()
            : base()
        {
        }

        #endregion

        #region IAnimateable implementation

        public override void Initialize(Animation animation, Vector2 position)
        {
            this.animation = animation;
            this.Position = position;
            this.Active = true;
            this.Health = 8;
        }

        #endregion

    }
}
