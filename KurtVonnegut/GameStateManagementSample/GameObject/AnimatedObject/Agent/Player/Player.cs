using System;
using System.Collections.Generic;
using System.Linq;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameStateManagementSample
{
    public class Player : Agent, IRotatable, IFireble, IGameObject, IAnimateable
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
        
        public Player()
        {
        }

        //properties
        public IList<Item> Inventory { get; private set; }

        public float Rotation { get; set; }

        public float PlayerMoveSpeed { get; set; }
                        

        // Amount of hit points that player has
        public int Health { get; set; }
        
        //methods
        public override void Initialize(Animation animation, Vector2 position)
        {
            this.animation = animation;
            this.Inventory = new List<Item>();
            this.PlayerMoveSpeed = INIT_MOVESPEED;
            this.Rotation = 0;
            // Set the starting position of the player around the middle of the screen and to the back
            this.Position = position;
            this.initialPos = position;

            // Set the player to be active
            this.Active = true;

            // Set the player health
            this.Health = DEF_HP;

            // Set the laser to fire every quarter second
            this.FireTime = TimeSpan.FromSeconds(FIRE_DELAY);
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
                rectangle2 = new Rectangle((int)solid.Position.X - solid.Width / 2, (int)solid.Position.Y - solid.Height / 2, solid.Width, solid.Height);
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

            this.animation.Position = this.Position;
            this.animation.Update(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            this.animation.Draw(spriteBatch, this);
            //spriteBatch.Draw(this.PlayerAnimation, this.Position, null, Color.White,this.Rotation, new Vector2(this.PlayerAnimation.Width/2, this.PlayerAnimation.Height/2) , 1f, SpriteEffects.None, 0f);
        }

        public void RotateTowards(Vector2 position)
        {
            float distanceX = this.Position.X - this.Width / 2 - position.X;
            float distanceY = this.Position.Y - this.Height / 2 - position.Y;
            this.Rotation = (float)Math.Atan2(distanceY, distanceX) - MathHelper.Pi;
        }

        public void PickUpItem(Item item)
        {
            this.Inventory.Add(item);
        }

        public void DropItem(Item item)
        {
            if (this.Inventory.Contains(item))
            {
                int index = this.Inventory.IndexOf(item);
                this.Inventory.RemoveAt(index);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
