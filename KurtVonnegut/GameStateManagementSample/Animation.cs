using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagementSample
{
    public class Animation
    {
        // Width of a given frame
        public int FrameWidth;

        // Height of a given frame
        public int FrameHeight;

        // The state of the Animation
        public bool Active;

        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        // Width of a given frame
        public Vector2 Position;

        // The image representing the collection of images used for animation
        private Texture2D spriteStrip;

        // The scale used to display the sprite strip
        private float scale;

        // The time since we last updated the frame
        private int elapsedTime;

        // The time we display a frame until the next one
        private int frameTime;

        // The number of frames that the animation contains
        private int frameCount;

        // The index of the current frame we are displaying
        private int currentFrame;

        // The color of the frame we will be displaying
        private Color color;

        // The area of the image strip we want to display
        private Rectangle sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        private Rectangle destinationRect = new Rectangle();

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            this.Looping = looping;
            this.Position = position;
            this.spriteStrip = texture;

            // Set the time to zero
            this.elapsedTime = 0;
            this.currentFrame = 0;

            // Set the Animation to active by default
            this.Active = true;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (this.Active == false)
            {
                return;
            }

            // Update the elapsed time
            this.elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (this.elapsedTime > this.frameTime)
            {
                // Move to the next frame
                this.currentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (this.currentFrame == this.frameCount)
                {
                    this.currentFrame = 0;
                    // If we are not looping deactivate the animation
                    if (this.Looping == false)
                    {
                        this.Active = false;
                    }
                }

                // Reset the elapsed time to zero
                this.elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            this.sourceRect = new Rectangle(this.currentFrame * this.FrameWidth, 0, this.FrameWidth, this.FrameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            this.destinationRect = new Rectangle((int)this.Position.X - (int)(this.FrameWidth * this.scale) / 2,
                (int)this.Position.Y - (int)(this.FrameHeight * this.scale) / 2,
                (int)(this.FrameWidth * this.scale),
                (int)(this.FrameHeight * this.scale));
        }

        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            // Only draw the animation when we are active
            if (this.Active)
            {
                spriteBatch.Draw(this.spriteStrip, this.destinationRect, this.sourceRect, Color.White, player.Rotation, new Vector2(this.FrameWidth / 2, this.FrameHeight / 2), SpriteEffects.None, 0f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (this.Active)
            {
                spriteBatch.Draw(this.spriteStrip, this.destinationRect, this.sourceRect, this.color);
            }
        }
    }
}