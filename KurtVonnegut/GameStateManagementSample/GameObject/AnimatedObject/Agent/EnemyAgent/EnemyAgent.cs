﻿namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class EnemyAgent
        : Agent
    {
        #region fields

        protected int damage;
        protected float speed;
        protected int value;

        #endregion

        #region properties

        public int Damage
        {
            get { return this.damage; }
            set
            {
                if (value <= 0)
                {
                    throw new System.ArgumentOutOfRangeException("Enemy damage must be positive int!");
                }
                this.damage = value;
            }
        }

        public float Speed
        {
            get { return this.speed; }
            set
            {
                if (value <= 0)
                {
                    throw new System.ArgumentOutOfRangeException("Speed must have positive value!");
                }
                this.speed = value;
            }
        }

        public int Value
        {
            get { return this.value; }
            set
            {
                if (value <= 0)
                {
                    throw new System.ArgumentOutOfRangeException("Value must be positive!");
                }
                this.value = value;
            }
        }

        #endregion

        #region constructor

        public EnemyAgent()
            : base()
        {
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            // The enemy always moves to the left so decrement it's xposition
            this.X -= this.Speed;

            // Update the position of the Animation
            this.animation.Position = this.Position;

            // Update Animation
            this.animation.Update(gameTime);

            // If the enemy is past the screen or its health reaches 0 then deactivateit
            if (this.Position.X < -this.Width || this.Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet fromthe
                // active game list
                this.Active = false;
            }
        }
    }
}
