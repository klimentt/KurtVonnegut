namespace DeBuggerGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;

    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //
        static public Color[] colors = new Color[] { Color.White, Color.Cyan, Color.AliceBlue, Color.Green, 
         Color.Yellow, Color.Orange, Color.Red, Color.Magenta, Color.Violet };

        // for intro
        Video video;
        VideoPlayer videoPlayer;
        Texture2D videoTexture;

        // image for static background
        Texture2D mainBackground;

        // player
        public Player player;
        public Animation PlayerAnimation;
        // player fire rate
        TimeSpan fireTime;
        TimeSpan previousFireTime;
        // player movement speed
        private float playerMoveSpeed;
                
        // keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        
        // enemies
        Texture2D Ant;
        Texture2D AntFire;
        Texture2D Moth;
        Texture2D MothFire;
        Texture2D Fly;
        Texture2D FlyFire;
        List<Enemy> enemies;
        // enemy spawn rate
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;


        // loot
        Texture2D Nana;
        Texture2D Cafe;
        Texture2D NDrink;
        Texture2D Board;
        Texture2D Mouse;
        Texture2D Monitor;
        Texture2D Coin;
        Texture2D Gem;                
        List<Item> loot;
        // loot spawn rate
        TimeSpan lootSpawnTime;
        TimeSpan previousLootSpawnTime;

        // projectiles
        Texture2D projectileTexture;
        List<Projectile> projectiles;
        Texture2D explosionTexture;
        Texture2D explosion2Texture;
        List<Animation> explosions;        
                

        
        
        // random number generator
        static public Random random;
                

        

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // initialize player
            player = new Player();

            // set player movement speed
            playerMoveSpeed = 2.7f;

            // initialize enemies list
            enemies = new List<Enemy>();

            // initialize loot list
            loot = new List<Item>();

            // set time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            previousLootSpawnTime = TimeSpan.Zero;

            // respawn times
            enemySpawnTime = TimeSpan.FromSeconds(0.6f);
            lootSpawnTime = TimeSpan.FromSeconds(1.5f);

            // Initialize our random number generator
            random = new Random();

            projectiles = new List<Projectile>();

            // set fire rate
            fireTime = TimeSpan.FromSeconds(0.55f);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // load video resources
            video = Content.Load<Video>("intro");
            videoPlayer = new VideoPlayer();

            // load the player resources
            PlayerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("maleFigure_walk");
            PlayerAnimation.Initialize(playerTexture, Vector2.Zero, playerTexture.Width / 8, playerTexture.Height, 8, 80, Color.White, 1, true);

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y
            + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(PlayerAnimation, playerPosition);

            // load sprite resources

            mainBackground = Content.Load<Texture2D>("circuitBoard");
            projectileTexture = Content.Load<Texture2D>("bolt");
            explosionTexture = Content.Load<Texture2D>("explosion");
            explosion2Texture = Content.Load<Texture2D>("explosion_2");

            // load animation resources
            Ant = Content.Load<Texture2D>("ant");
            AntFire = Content.Load<Texture2D>("ant_ghost");
            Moth = Content.Load<Texture2D>("moth");
            MothFire = Content.Load<Texture2D>("moth_ghost");
            Fly = Content.Load<Texture2D>("fly");
            FlyFire = Content.Load<Texture2D>("fly_ghost");
            Nana = Content.Load<Texture2D>("banana");
            Cafe = Content.Load<Texture2D>("coffee");
            NDrink = Content.Load<Texture2D>("energy_drink");
            Board = Content.Load<Texture2D>("keyboard");
            Mouse = Content.Load<Texture2D>("mouse");
            Monitor = Content.Load<Texture2D>("monitor");
            Coin = Content.Load<Texture2D>("coin");
            Gem = Content.Load<Texture2D>("gem");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // play intro
            //if (videoPlayer.State == MediaState.Stopped)
            //{
            //    videoPlayer.IsLooped = false;
            //    videoPlayer.Play(video);
            //}

         
            
            // save previous state of keyboard
            previousKeyboardState = currentKeyboardState;

            // read current state of the keyboard and store it
            currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            // update player
            UpdatePlayer(gameTime);

            // update enemies
            UpdateEnemies(gameTime);

            // update loot
            UpdateLoot(gameTime);

            // update collision
            UpdateCollision();

            // update projectiles
            UpdateProjectiles();

            // update explosions
            //UpdateExplosions(gameTime);
            


            // ...


            base.Update(gameTime);
        }

        private void AddEnemy()
        {
            // create animation object
            Animation enemyAnimation = new Animation();

            // randomly generate enemy position 
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + Ant.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));


            int r = random.Next(1000);

            Enemy enemy = new Enemy();
            
            Color col = colors[random.Next(colors.Length - 1)];

            switch (r % 6)
            {
                case 0:
                    {
                        enemy = new Ant();
                        enemyAnimation.Initialize(Ant, Vector2.Zero, 32, 32, 16, 1000, col, 1, true);
                    }
                    break;
                case 1:
                    {
                        enemy = new FireAnt();
                        enemyAnimation.Initialize(AntFire, Vector2.Zero, 32, 32, 16, 250, col, 1, true);
                    }
                    break;
                case 2:
                    {
                        enemy = new Moth();
                        enemyAnimation.Initialize(Moth, Vector2.Zero, 40, 40, 12, 1000, col, 1, true);

                    }
                    break;
                case 3:
                    {
                        enemy = new FireMoth();
                        enemyAnimation.Initialize(MothFire, Vector2.Zero, 40, 40, 12, 250, col, 1, true);

                    }
                    break;
                case 4:
                    {
                        enemy = new Fly();
                        enemyAnimation.Initialize(Fly, Vector2.Zero, 60, 60, 24, 1000, col, 0.5f, true);

                    }
                    break;
                case 5:
                    {
                        enemy = new FireFly();
                        enemyAnimation.Initialize(FlyFire, Vector2.Zero, 60, 60, 24, 250, col, 0.4f, true);

                    }
                    break;
            }

            enemy.Initialize(enemyAnimation, position);
            enemies.Add(enemy);
        }

        private void AddLoot()
        {
            // create animation object
            Animation lootAnimation = new Animation();

            // randomly generate loot position 
            Vector2 position = new Vector2(random.Next(GraphicsDevice.Viewport.Width), random.Next(GraphicsDevice.Viewport.Height-60, GraphicsDevice.Viewport.Height - 20));


            int r = random.Next(1000);
            
            Item lootItem = new Item();

            switch (r % 8)
            {
                case 0:
                    {
                        lootItem = new Banana();
                        lootAnimation.Initialize(Nana, Vector2.Zero, 40, 40, 4, 1200, Color.White, 1, true);
                    }
                    break;
                case 1:
                    {
                        lootItem = new Keyboard();
                        lootAnimation.Initialize(Board, Vector2.Zero, 40, 40, 4, 1200, Color.White, 1, true);
                    }
                    break;
                case 2:
                    {
                        lootItem = new Mouse();
                        lootAnimation.Initialize(Mouse, Vector2.Zero, 40, 40, 4, 1200, Color.White, 1, true);
                    }
                    break;
                case 3:
                    {
                        lootItem = new Coffee();
                        lootAnimation.Initialize(Cafe, Vector2.Zero, 40, 40, 4, 1200, Color.White, 1, true);
                    }
                    break;
                case 4:
                    {
                        lootItem = new EnergyDrink();
                        lootAnimation.Initialize(NDrink, Vector2.Zero, 40, 40, 4, 1200, Color.White, 0.8f, true);

                    }
                    break;
                case 5:
                    {
                        lootItem = new Monitor();
                        lootAnimation.Initialize(Monitor, Vector2.Zero, 40, 40, 4, 1200, Color.White, 0.8f, true);

                    }
                    break;
                case 6:
                    {
                        lootItem = new Coin();
                        lootAnimation.Initialize(Coin, Vector2.Zero, 60, 30, 12, 90, Color.White, 1, true);
                    }
                    break;
                case 7:
                    {
                        lootItem = new Gem();
                        lootAnimation.Initialize(Gem, Vector2.Zero, 60, 60, 12, 40, Color.Aquamarine, 1, true);
                    }
                    break;
            }

            lootItem.Initialize(lootAnimation, position);
            loot.Add(lootItem);
        }

        private void AddProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position);
            projectiles.Add(projectile);
        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();

                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();

            explosion.Initialize(explosion2Texture, position, 40, 40, 12, 45, Color.White, 1f, false);
            
            explosions.Add(explosion);
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].ActiveState == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                player.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                player.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                player.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                player.Y += playerMoveSpeed;
            }

            // make sure that the player does not go out of bounds
            player.X = MathHelper.Clamp(player.X, 20, GraphicsDevice.Viewport.Width - player.Width + 15);
            player.Y = MathHelper.Clamp(player.Y, 45, GraphicsDevice.Viewport.Height - player.Height + 40);

            // Fire only every interval we set as the fireTime
            if (gameTime.TotalGameTime - previousFireTime > fireTime)
            {
                // Reset our current time
                previousFireTime = gameTime.TotalGameTime;

                // Add the projectile, but add it to the front and center of the player
                AddProjectile(player.Position + new Vector2(player.Width / 2, 0));
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            // spawn a new enemy enemy every enemySpawnTime seconds
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                // add an enemy
                AddEnemy();
            }

            // update enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);

                if (enemies[i].Active == false)
                {
                    //AddExplosion(enemies[i].Position);
                    
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdateLoot(GameTime gameTime)
        {
            // spawn new loot every lootSpawnTime seconds
            if (gameTime.TotalGameTime - previousLootSpawnTime > lootSpawnTime)
            {
                previousLootSpawnTime = gameTime.TotalGameTime;

                // add loot
                AddLoot();
            }

            // update loot
            for (int i = loot.Count - 1; i >= 0; i--)
            {
                loot[i].Update(gameTime);

                if (loot[i].Active == false)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdateCollision()
        {
            // use rectangle's built-in intersect function to
            // determine if two objects overlap
            Rectangle rectangle1;
            Rectangle rectangle2;
                        
            rectangle1 = new Rectangle((int)player.Position.X,
            (int)player.Position.Y,
            player.Width,
            player.Height);

            // do collision between player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                rectangle2 = new Rectangle((int)enemies[i].Position.X,
                (int)enemies[i].Position.Y,
                enemies[i].Width,
                enemies[i].Height);

                // determine if the two objects collided 
                if (rectangle1.Intersects(rectangle2))
                {
                    // subtract health from player based on
                    // enemy damage
                    player.Health -= enemies[i].Damage;
                    
                    // if player health non-positive - die
                    if (player.Health <= 0)
                    {
                        player.Active = false;
                    }
                }
            }

            // projectile vs enemy collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    // create rectangles to determine if collision occurred
                    rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

                    rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
                    (int)enemies[j].Position.Y - enemies[j].Height / 2,
                    enemies[j].Width, enemies[j].Height);

                    // determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        enemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive);

            // play video

            if (videoPlayer.State != MediaState.Stopped)
                videoTexture = videoPlayer.GetTexture();

            Rectangle screen = new Rectangle
                (
                GraphicsDevice.Viewport.X,
                GraphicsDevice.Viewport.Y,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height
                );

            if (videoTexture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(videoTexture, screen, Color.White);
                spriteBatch.End();
            }


            // draw background
            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

            // draw player            
            player.Draw(spriteBatch);

            // draw enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

            // draw loot
            for (int i = 0; i < loot.Count; i++)
            {
                loot[i].Draw(spriteBatch);
            }

            // draw projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }

            //// draw explosions
            //for (int i = 0; i < explosions.Count; i++)
            //{
            //    explosions[i].Draw(spriteBatch);
            //}

            // ...

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
