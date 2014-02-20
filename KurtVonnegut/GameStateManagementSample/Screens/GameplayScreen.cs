#region File Description

//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace GameStateManagementSample
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        public GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly Player player;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        
        //enemy
        private Texture2D enemyTexture;
        private readonly List<Enemy> enemies;
        // The rate at which the enemies appear
        private readonly TimeSpan enemySpawnTime;
        private TimeSpan previousSpawnTime;

        // A random number generator
        private readonly Random random;
        
        //projectiles
        private Texture2D projectileTexture;
        private readonly List<Projectile> projectiles;
        
        //solid objects
        private Texture2D solidTexture;
        private readonly List<Solid> solids;
        
        //handle effects
        private Texture2D explosionTexture;
        private readonly List<Animation> explosions;
        
        //Number that holds the player score
        private int score;
        // The font used to display UI elements
        private SpriteFont font;
        
        #region Fields
        
        private ContentManager content;
        private SpriteFont gameFont;
        
        //Vector2 playerPosition = new Vector2(100, 100);
        //Vector2 enemyPosition = new Vector2(100, 100);

        //Random random = new Random();

        private float pauseAlpha;

        private readonly InputAction pauseAction;

        #endregion
        
        #region Initialization
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(1.5);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
            this.pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
            // TODO: Add your initialization logic here
            this.player = new Player();
            
            // Initialize the enemies list
            this.enemies = new List<Enemy>();
            // Set the time keepers to zero
            this.previousSpawnTime = TimeSpan.Zero;
            // Used to determine how fast enemy respawns
            this.enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            
            //initialize projectiles
            this.projectiles = new List<Projectile>();
            
            //initialize effect explosions
            this.explosions = new List<Animation>();

            //initialize solid objects
            this.solids = new List<Solid>();
            this.solids.Add(new Solid());
            
            // Initialize our random number generator
            this.random = new Random();
            
            this.score = 0;
        }
        
        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (this.content == null)
                {
                    this.content = new ContentManager(this.ScreenManager.Game.Services, "Content");
                }

                this.gameFont = this.content.Load<SpriteFont>("gamefont");
                // Create a new SpriteBatch, which can be used to draw textures.
                this.spriteBatch = new SpriteBatch(this.ScreenManager.GraphicsDevice);
                
                // TODO: use this.Content to load your game content here
                
                //player
                Animation playerAnimation = new Animation();
                Texture2D playerTexture = this.content.Load<Texture2D>("shipAnimation");
                playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

                Vector2 playerPosition = new Vector2(this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y +
                                                                                                                 this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
                this.player.Initialize(playerAnimation, playerPosition);
                
                //enemy
                this.enemyTexture = this.content.Load<Texture2D>("mineAnimation");
                
                //projectile
                this.projectileTexture = this.content.Load<Texture2D>("laser");
                
                //effect explosions
                this.explosionTexture = this.content.Load<Texture2D>("explosion");
                
                //solid objects texture
                this.solidTexture = this.content.Load<Texture2D>("solid");

                this.solids[0].Initialize(this.solidTexture, new Vector2(400,300));
                
                this.font = this.content.Load<SpriteFont>("gameFont");
                
                this.ScreenManager.Game.ResetElapsedTime();
            }
            #if WINDOWS_PHONE
if (Microsoft.Phone.Shell.PhoneApplicationService.Current.State.ContainsKey("PlayerPosition"))
{
playerPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"];
enemyPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"];
}
            #endif
        }
        
        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            this.content.Unload();
            #if WINDOWS_PHONE
Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("PlayerPosition");
Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("EnemyPosition");
            #endif
        }

        #endregion
        
        #region Update and Draw
        
        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                this.pauseAlpha = Math.Min(this.pauseAlpha + 1f / 32, 1);
            }
            else
            {
                this.pauseAlpha = Math.Max(this.pauseAlpha - 1f / 32, 0);
            }
            
            if (this.IsActive)
            {
                // TODO: Add your update logic here
                // Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
                this.previousKeyboardState = this.currentKeyboardState;
                this.previousMouseState = this.currentMouseState;

                // Read the current state of the keyboard and mouse and store it
                this.currentKeyboardState = Keyboard.GetState();
                this.currentMouseState = Mouse.GetState();
                
                //Update the player
                this.player.Update(this.currentKeyboardState, this.currentMouseState, this.ScreenManager, gameTime, this.solids);
                if (this.currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    if (gameTime.TotalGameTime - Projectile.previousFireTime > Projectile.fireTime)
                    {
                        // Reset our current time
                        Projectile.previousFireTime = gameTime.TotalGameTime;
                        
                        // Add the projectile, but add it to the front and center of the player
                        this.AddProjectile(player.Position + new Vector2(-player.Width / 2, -player.Height / 2));
                    }
                }
                //collision detection update
                this.UpdateCollision();
                //update enemies and add new ones/remove old
                this.UpdateEnemies(gameTime);
                this.UpdateProjectiles();
                this.UpdateExplosions(gameTime);
            }
        }
        
        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex player;
            if (this.pauseAction.Evaluate(input, this.ControllingPlayer, out player))
            {
                this.ScreenManager.AddScreen(new PauseMenuScreen(), this.ControllingPlayer);
            }
        }
        
        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            this.ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.CornflowerBlue, 0, 0);
            
            // TODO: Add your drawing code here
            this.spriteBatch.Begin();
            foreach (var solid in this.solids)
            {
                solid.Draw(this.spriteBatch);
            }
            //Draw player
            this.player.Draw(this.spriteBatch);
            //draw projectiles
            for (int i = 0; i < this.projectiles.Count; i++)
            {
                this.projectiles[i].Draw(this.spriteBatch);
            }
            // Draw the Enemies
            for (int i = 0; i < this.enemies.Count; i++)
            {
                this.enemies[i].Draw(this.spriteBatch);
            }
            // Draw the explosions
            for (int i = 0; i < this.explosions.Count; i++)
            {
                this.explosions[i].Draw(this.spriteBatch);
            }
            // Draw the score
            this.spriteBatch.DrawString(this.font, string.Format("Score: {0}", this.score), new Vector2(this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
            // Draw the player health
            this.spriteBatch.DrawString(this.font, string.Format("Health: {0}", this.player.Health), new Vector2(this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y + 30), Color.White);
            this.spriteBatch.End();
            base.Draw(gameTime);
            if (this.TransitionPosition > 0 || this.pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - this.TransitionAlpha, 1f, this.pauseAlpha / 2);

                this.ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        
        #endregion
        
        private void AddEnemy()
        {
            // Create the animation object
            Animation enemyAnimation = new Animation();
            
            // Initialize the animation with the correct animation information
            enemyAnimation.Initialize(this.enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
            
            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(this.ScreenManager.GraphicsDevice.Viewport.Width + this.enemyTexture.Width / 2, this.random.Next(100, this.ScreenManager.GraphicsDevice.Viewport.Height - 100));
            
            // Create an enemy
            Enemy enemy = new Enemy();
            
            // Initialize the enemy
            enemy.Initialize(enemyAnimation, position);

            // Add the enemy to the active enemies list
            this.enemies.Add(enemy);
        }
        
        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - this.previousSpawnTime > this.enemySpawnTime)
            {
                this.previousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                this.AddEnemy();
            }
            
            // Update the Enemies
            for (int i = this.enemies.Count - 1; i >= 0; i--)
            {
                this.enemies[i].Update(gameTime);
                
                if (this.enemies[i].Active == false)
                {
                    if (this.enemies[i].Health <= 0)
                    {
                        // Add an explosion
                        this.AddExplosion(enemies[i].Position);
                        //Add to the player's score
                        this.score += this.enemies[i].Value;
                    }
                    else
                    {
                        this.score -= this.enemies[i].Value;
                    }
                    this.enemies.RemoveAt(i);
                }
            }
        }
        
        private void UpdateCollision()
        {
            // Use the Rectangle's built-in intersect function to
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;
            
            // Projectile vs Enemy Collision
            for (int i = 0; i < this.projectiles.Count; i++)
            {
                for (int j = 0; j < this.enemies.Count; j++)
                {
                    // Create the rectangles we need to determine if we collided with each other
                    rectangle1 = new Rectangle((int)this.projectiles[i].Position.X - this.projectiles[i].Width / 2, (int)this.projectiles[i].Position.Y - this.projectiles[i].Height / 2, this.projectiles[i].Width, this.projectiles[i].Height);
                    
                    rectangle2 = new Rectangle((int)this.enemies[j].Position.X - this.enemies[j].Width / 2, (int)this.enemies[j].Position.Y - this.enemies[j].Height / 2, this.enemies[j].Width, this.enemies[j].Height);
                    
                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        this.enemies[j].Health -= this.projectiles[i].Damage;
                        this.projectiles[i].Active = false;
                    }
                }
            }
            
            // Only create the rectangle once for the player
            rectangle1 = new Rectangle((int)this.player.Position.X - this.player.Width / 2, (int)this.player.Position.Y - this.player.Height / 2, this.player.Width, this.player.Height);
            
            // Do the collision between the player and the enemies
            for (int i = 0; i < this.enemies.Count; i++)
            {
                rectangle2 = new Rectangle((int)this.enemies[i].Position.X, (int)this.enemies[i].Position.Y, this.enemies[i].Width, this.enemies[i].Height);
                
                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    this.player.Health -= this.enemies[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    this.enemies[i].Health = 0;
                    
                    // If the player health is less than zero we died
                    if (this.player.Health <= 0)
                    {
                        this.player.Active = false;
                    }
                }
            }

            foreach (var solid in this.solids)
            {
                rectangle1 = new Rectangle((int)solid.Position.X, (int)solid.Position.Y, solid.Width, solid.Height);
                
                // Do the collision between the solids and the enemies
                for (int i = 0; i < this.enemies.Count; i++)
                {
                    rectangle2 = new Rectangle((int)this.enemies[i].Position.X, (int)this.enemies[i].Position.Y, this.enemies[i].Width, this.enemies[i].Height);
                    
                    // Determine if the two objects collided with each
                    // other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        this.enemies[i].Health = 0;
                    }
                }
                for (int i = 0; i < this.projectiles.Count; i++)
                {
                    rectangle2 = new Rectangle((int)this.projectiles[i].Position.X - this.projectiles[i].Width / 2, (int)this.projectiles[i].Position.Y - this.projectiles[i].Height / 2, this.projectiles[i].Width, this.projectiles[i].Height);
                    
                    // Determine if the two objects collided with each
                    // other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        this.projectiles[i].Active = false;
                    }
                }
            }
        }
        
        private void AddProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(this.ScreenManager.GraphicsDevice.Viewport, this.projectileTexture, position, this.player);
            this.projectiles.Add(projectile);
        }
        
        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = this.projectiles.Count - 1; i >= 0; i--)
            {
                this.projectiles[i].Update();
                
                if (this.projectiles[i].Active == false)
                {
                    this.projectiles.RemoveAt(i);
                }
            }
        }
        
        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(this.explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
            this.explosions.Add(explosion);
        }
        
        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = this.explosions.Count - 1; i >= 0; i--)
            {
                this.explosions[i].Update(gameTime);
                if (this.explosions[i].Active == false)
                {
                    this.explosions.RemoveAt(i);
                }
            }
        }
    }
}