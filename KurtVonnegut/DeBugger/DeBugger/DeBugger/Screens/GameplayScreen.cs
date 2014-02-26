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
using Microsoft.Xna.Framework.Audio;
using DeBugger.Screens;

#endregion

namespace DeBugger
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        public GraphicsDeviceManager Graphics;
        private SpriteBatch spriteBatch;
        private readonly Player player;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private MouseState currentMouseState;
        private MouseState previousMouseState;

        //enemy
        private Texture2D enemyTexture;
        List<Texture2D> enemyAnimationTextures;
        private readonly List<RotatingEnemy> enemies;
        // The rate at which the enemies appear
        private readonly TimeSpan enemySpawnTime;
        private TimeSpan previousSpawnTime;
        private SoundEffect roachSmashed;


        // A random number generator
        public static Random Random;

        //projectiles
        private Texture2D projectileTexture;
        private readonly List<Projectile> projectiles;
        private Texture2D turretProjTexture;

        //solid objects
        private Texture2D solidTexture;
        private List<Solid> solids;
        private List<Turret> turrets;
        private SoundEffect turretSound;
        private Texture2D turretTexture;

        //handle effects
        private Texture2D explosionTexture;
        private readonly List<Animation> explosions;
        private SoundEffect shotSound;

        //Number that holds the player score
        private int xp;

        // String that holds the player level
        private string lvl;
        // The font used to display UI elements
        private SpriteFont font;

        //Gameplay Background

        Microsoft.Xna.Framework.Rectangle mainframe;
        Texture2D background;

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
            this.enemies = new List<RotatingEnemy>();
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
            this.solids.Add(new Solid());//TODO: temporary

            //initialize turrets
            this.turrets = new List<Turret>();
            this.turrets.Add(new Turret());

            // Initialize our random number generator
            GameplayScreen.Random = new Random();

            this.xp = 0;
            this.lvl = "";
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

                this.roachSmashed = content.Load<SoundEffect>(("Sounds\\roach_smashed"));
                this.shotSound = content.Load<SoundEffect>(("Sounds\\shot"));
                this.turretSound = content.Load<SoundEffect>(("Sounds\\turretLaunch"));
                // TODO: use this.Content to load your game content here

                //player
                Animation playerAnimation = new Animation();
                string plTexture = "player_2_animation";
                if (CharacterSelectionScreen.currentRace == CharacterSelectionScreen.Race.Administrator)
                {
                    plTexture = "player_2_animation";
                }
                else if (CharacterSelectionScreen.currentRace == CharacterSelectionScreen.Race.Designer)
                {
                    plTexture = "femaleFigure_walk";
                }
                else
                {
                    plTexture = "maleFigure_walk";
                }
                Texture2D playerTexture = this.content.Load<Texture2D>(plTexture);
                int playerFrames = 8;
                playerAnimation.Initialize(playerTexture, Vector2.Zero, playerTexture.Width / playerFrames, playerTexture.Height, playerFrames, 30, Color.White, 1f, true);

                Vector2 playerPosition = new Vector2(this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y +
                                                                                                                 this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
                this.player.Initialize(playerAnimation, playerPosition);

                //background screen
                this.mainframe = new Microsoft.Xna.Framework.Rectangle(0, 0, this.ScreenManager.GraphicsDevice.Viewport.Width, this.ScreenManager.GraphicsDevice.Viewport.Height);
                this.background = this.content.Load<Texture2D>("circuitBoard");

                //enemy
                this.enemyTexture = this.content.Load<Texture2D>("roach");
                enemyAnimationTextures = new List<Texture2D>();
                enemyAnimationTextures.Add(this.content.Load<Texture2D>("ant"));
                enemyAnimationTextures.Add(this.content.Load<Texture2D>("ant_ghost"));
                enemyAnimationTextures.Add(this.content.Load<Texture2D>("fly"));
                enemyAnimationTextures.Add(this.content.Load<Texture2D>("fly_ghost"));
                enemyAnimationTextures.Add(this.content.Load<Texture2D>("moth"));
                enemyAnimationTextures.Add(this.content.Load<Texture2D>("moth_ghost"));
                enemyAnimationTextures.Add(this.content.Load<Texture2D>("roach"));
                enemyAnimationTextures.Add(this.content.Load<Texture2D>("roach_ghost"));

                //projectile
                this.projectileTexture = this.content.Load<Texture2D>("laser");
                this.turretProjTexture = this.content.Load<Texture2D>("turretProjectile");

                //effect explosions
                this.explosionTexture = this.content.Load<Texture2D>("explosion_2");

                //solid objects texture
                this.solidTexture = this.content.Load<Texture2D>("solid");
                //turret texture
                this.turretTexture = this.content.Load<Texture2D>("turret");
                this.solids[0].Initialize(this.solidTexture, new Vector2(400, 300), 1);

                //add turret that uses the same texture from solid
                this.turrets[0].Initialize(this.turretTexture, new Vector2(600, 360), 1, -1.57079633f); //1.57079633f

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
                //fire projectiles from player when needed
                if (this.currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (gameTime.TotalGameTime - this.player.PreviousFireTime > this.player.FireTime)
                    {
                        // Reset our current time
                        this.player.PreviousFireTime = gameTime.TotalGameTime;

                        // Add the projectile, but add it to the front and center of the player
                        this.AddPlayerProjectile(player.Position + new Vector2(-player.Width / 2, -player.Height / 2));
                    }
                }

                foreach (var tur in this.turrets)
                {
                    if (gameTime.TotalGameTime - tur.PreviousFireTime > tur.FireTime)
                    {
                        // Reset our current time
                        tur.PreviousFireTime = gameTime.TotalGameTime;

                        // Add the projectile, but add it to the front and center of the player
                        this.AddTurretProjectile(tur);
                    }
                }

                //collision detection update
                this.UpdateCollision(gameTime);
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
            this.ScreenManager.GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            spriteBatch.Draw(background, mainframe, Microsoft.Xna.Framework.Color.Gray);

            //draw projectiles
            for (int i = 0; i < this.projectiles.Count; i++)
            {
                this.projectiles[i].Draw(this.spriteBatch);
            }
            //draw turrets 
            foreach (var tur in this.turrets)
            {
                tur.Draw(this.spriteBatch);
            }
            //draw solids
            foreach (var solid in this.solids)
            {
                solid.Draw(this.spriteBatch);
            }
            //Draw player
            this.player.Draw(this.spriteBatch);

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
            this.spriteBatch.DrawString(this.font, string.Format("XP: {0}", this.xp), new Vector2(this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
            // Draw the player level
            this.spriteBatch.DrawString(this.font, string.Format("Level: {0}", this.lvl), new Vector2(this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y + 30), Color.White);
            // Draw the player health
            this.spriteBatch.DrawString(this.font, string.Format("Health: {0}", this.player.Health), new Vector2(this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, this.ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y + 60), Color.White);

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
            int enemyFrameCount = 8;



            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(GameplayScreen.Random.Next(250, this.ScreenManager.GraphicsDevice.Viewport.Width + this.enemyTexture.Width / 2), GameplayScreen.Random.Next(100, this.ScreenManager.GraphicsDevice.Viewport.Height - 100));

            // Create an enemy

            RotatingEnemy enemy;

            switch (Random.Next(9) % 8)
            {
                case 1:
                    enemy = new Roach();
                    this.enemyTexture = this.enemyAnimationTextures[6];
                    break;
                case 2:
                    enemy = new FireRoach();
                    this.enemyTexture = this.enemyAnimationTextures[7];
                    break;
                case 3:
                    enemy = new Fly();
                    this.enemyTexture = this.enemyAnimationTextures[2];
                    break;
                case 4:
                    enemy = new FireFly();
                    this.enemyTexture = this.enemyAnimationTextures[3];
                    break;
                //case 5:
                //    break;
                //case 6:
                //    break;
                //case 7:
                //    break;
                //case 8:
                //    break;
                default:
                    enemy = new Roach();
                    break;

            }

            enemyAnimation.Initialize(this.enemyTexture, Vector2.Zero, this.enemyTexture.Width / enemyFrameCount, this.enemyTexture.Height, enemyFrameCount, 30, Color.White, 1f, true);

            // Initialize the enemy
            enemy.Initialize(enemyAnimation, position, 0, 300);

            // Add the enemy to the active enemies list
            this.enemies.Add(enemy);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - this.previousSpawnTime > this.enemySpawnTime)
            {
                this.previousSpawnTime = gameTime.TotalGameTime;

                //Add an Enemy
                this.AddEnemy();
            }

            // Update the Enemies
            for (int i = this.enemies.Count - 1; i >= 0; i--)
            {
                //we save the prev position of the object
                Vector2 prevPos = new Vector2(this.enemies[i].Position.X, this.enemies[i].Position.Y);
                this.enemies[i].Update(gameTime);
                if (CollidesWithObject(this.enemies[i], i))
                {
                    this.enemies[i].Position = prevPos;
                }
                if (this.enemies[i].Active == false)
                {
                    if (this.enemies[i].Health <= 0)
                    {
                        // Add an explosion
                        this.AddExplosion(enemies[i].Position);
                        //Add to the player's score
                        this.xp += this.enemies[i].Value;
                    }
                    else
                    {
                        this.xp -= this.enemies[i].Value;
                    }
                    this.enemies.RemoveAt(i);


                }
            }
            //check if player is in aggrorange
            for (int i = 0; i < enemies.Count; i++)
            {
                if (Vector2.Distance(this.enemies[i].Position, this.player.Position) <= this.enemies[i].AggroRange)
                {
                    enemies[i].IsInAggroRange = true;
                }
                if (enemies[i].IsInAggroRange)
                {
                    enemies[i].RotateTowards(player.Position);
                }
            }

            // Update levels
            if (this.xp < 100)
            {
                this.lvl = "Noob Ninja";
            }
            else if (this.xp > 100 && this.xp < 300)
            {
                this.lvl = "Ninja Disciple";
            }
            else if (this.xp > 300 & this.xp < 600)
            {
                this.lvl = "Ninja Acolyte";
            }
            else if (this.xp > 600 & this.xp < 1000)
            {
                this.lvl = "Master Ninja";
            }
            else if (this.xp > 1000)
            {
                this.lvl = "Master DeBugger";
            }
        }

        private bool CollidesWithObject(IGameObject obj, int index = 0)
        {
            Rectangle rectangle1 = new Rectangle((int)obj.Position.X - obj.Width / 2, (int)obj.Position.Y - obj.Height / 2, obj.Width, obj.Height);
            Rectangle rectangle2;
            //check with enemies 
            //for (int j = index + 1; j < this.enemies.Count; j++)
            //{
            //    rectangle2 = new Rectangle((int)this.enemies[j].Position.X - this.enemies[j].Width / 2, (int)this.enemies[j].Position.Y - this.enemies[j].Height / 2, this.enemies[j].Width, this.enemies[j].Height);
            //    if (rectangle1.Intersects(rectangle2))
            //    {
            //        return true;
            //    }
            //}
            //check with walls
            for (int j = 0; j < this.solids.Count; j++)
            {
                //if (this.solids[j] is FlyingEnemy)
                //{
                //    continue;
                //}
                rectangle2 = new Rectangle((int)this.solids[j].Position.X - this.solids[j].Width / 2, (int)this.solids[j].Position.Y - this.solids[j].Height / 2, this.solids[j].Width, this.solids[j].Height);
                if (rectangle1.Intersects(rectangle2))
                {
                    return true;
                }
            }
            //check with turrets
            for (int j = 0; j < this.turrets.Count; j++)
            {
                rectangle2 = new Rectangle((int)this.turrets[j].Position.X - this.turrets[j].Width / 2, (int)this.turrets[j].Position.Y - this.turrets[j].Height / 2, this.turrets[j].Width, this.turrets[j].Height);
                if (rectangle1.Intersects(rectangle2))
                {
                    return true;
                }
            }
            return false;

        }

        private void UpdateCollision(GameTime gameTime)
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
                rectangle2 = new Rectangle((int)this.enemies[i].Position.X - this.enemies[i].Width / 2, (int)this.enemies[i].Position.Y - this.enemies[i].Height / 2, this.enemies[i].Width, this.enemies[i].Height);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {

                    // Subtract the health from the player based on
                    // the enemy damage
                    if (gameTime.TotalGameTime - this.enemies[i].PreviousFireTime > this.enemies[i].FireTime)
                    {
                        this.player.Health -= this.enemies[i].Damage;
                        this.enemies[i].PreviousFireTime = gameTime.TotalGameTime;
                    }

                    // Since the enemy collided with the player
                    // destroy it
                    //this.enemies[i].Health = 0; 

                    // If the player health is less than zero we died
                    if (this.player.Health <= 0)
                    {
                        this.player.Active = false;
                    }
                }
            }

            foreach (var solid in this.solids)
            {
                rectangle1 = new Rectangle((int)solid.Position.X - solid.Width / 2, (int)solid.Position.Y - solid.Height / 2, solid.Width, solid.Height);

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

        private void AddPlayerProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(this.ScreenManager.GraphicsDevice.Viewport, this.projectileTexture, position, this.player);
            this.projectiles.Add(projectile);
            SoundCaller shotFired = new SoundCaller(this.shotSound);
        }

        private void AddTurretProjectile(Turret tur)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(this.ScreenManager.GraphicsDevice.Viewport, this.turretProjTexture, new Vector2(tur.Position.X, tur.Position.Y), tur);
            this.projectiles.Add(projectile);
            SoundCaller turretShotFired = new SoundCaller(this.turretSound);
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
            int explosionFrameCount = 12;
            explosion.Initialize(this.explosionTexture, position, this.explosionTexture.Width / explosionFrameCount, this.explosionTexture.Height, explosionFrameCount, 45, Color.White, 1f, false);
            this.explosions.Add(explosion);
            SoundCaller explosionSound = new SoundCaller(this.roachSmashed);
            //
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