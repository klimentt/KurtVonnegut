using System;
using System.Collections.Generic;
using System.Linq;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameStateManagementSample
{
    public class Player : IRotatable, IFireble, IGameObject
    {
        private const int DEF_HP = 100;
        private const float INIT_MOVESPEED = 8.0f;
        private const float FIRE_DELAY = 0.15f;

        //fields
        private Vector2 initialPos;
        //constructors

        // The rate of fire of the projectile laser
        public TimeSpan FireTime { get; set; }
        public TimeSpan PreviousFireTime { get; set; }
        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position { get; set; }
        public Player()
        {
            this.PlayerMoveSpeed = INIT_MOVESPEED;
            this.Rotation = 0;
        }

        //properties
        public float Rotation { get; set; }

        public float PlayerMoveSpeed { get; set; }

        public Animation PlayerAnimation { get; set; }

        // State of the player
        public bool Active { get; set; }

        // Amount of hit points that player has
        public int Health { get; set; }

        // Get the width of the player ship
        public int Width
        {
            get
            {
                return this.PlayerAnimation.FrameWidth;
            }
        }

        // Get the height of the player ship
        public int Height
        {
            get
            {
                return this.PlayerAnimation.FrameHeight;
            }
        }

        //methods
        public void Initialize(Animation animation, Vector2 position)
        {
            this.PlayerAnimation = animation;

            // Set the starting position of the player around the middle of the screen and to the back
            this.Position = position;
            this.initialPos = position;

            // Set the player to be active
            this.Active = true;

            // Set the player health
            this.Health = DEF_HP;

            // Set the laser to fire every quarter second
            FireTime = TimeSpan.FromSeconds(FIRE_DELAY);
        }

        public void Update(KeyboardState currentKeyboardState, MouseState currentMouseState, ScreenManager game, GameTime gameTime, List<Solid> solids)
        {
            Vector2 oldPosition = this.Position;
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                this.Position -=  new Vector2(this.PlayerMoveSpeed, 0);
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                this.Position += new Vector2(this.PlayerMoveSpeed, 0);
            }
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                this.Position -= new Vector2(0, this.PlayerMoveSpeed);
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                this.Position += new Vector2(0, this.PlayerMoveSpeed);
            }
            Rectangle rectangle1;
            Rectangle rectangle2;
            rectangle1 = new Rectangle((int)this.Position.X - this.Width, (int)this.Position.Y - this.Height, this.Width, this.Height);

            foreach (var solid in solids)
            {
                rectangle2 = new Rectangle((int)solid.Position.X , (int)solid.Position.Y, solid.Width, solid.Height);
                if (rectangle1.Intersects(rectangle2))
                {
                    this.Position = oldPosition;
                    break;
                }
            }

            if (this.Health <= 0)
            {
                this.Health = DEF_HP;
                this.Position = this.initialPos;
            }
            //rotates towards the mouse
            RotateTowards(new Vector2(currentMouseState.X, currentMouseState.Y));

            this.Position = new Vector2(MathHelper.Clamp(this.Position.X, this.Width, game.GraphicsDevice.Viewport.Width), MathHelper.Clamp(this.Position.Y, this.Height, game.GraphicsDevice.Viewport.Height)); ;
            
            this.PlayerAnimation.Position = this.Position;
            this.PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.PlayerAnimation.Draw(spriteBatch, this);
            //spriteBatch.Draw(this.PlayerAnimation, this.Position, null, Color.White,this.Rotation, new Vector2(this.PlayerAnimation.Width/2, this.PlayerAnimation.Height/2) , 1f, SpriteEffects.None, 0f);
        }

        public void RotateTowards(Vector2 position)
        {
            float distanceX = this.Position.X - this.Width / 2 - position.X;
            float distanceY = this.Position.Y - this.Height / 2 - position.Y;
            this.Rotation = (float)Math.Atan2(distanceY, distanceX) - MathHelper.Pi;
        }
    }
}
