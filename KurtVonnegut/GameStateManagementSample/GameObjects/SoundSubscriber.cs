namespace DeBuggerGame
{
    using System;
    using System.Threading;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class SoundSubscriber : Sound
    {
        private ContentManager content;

        public void Subscribe(SoundPublisher publisher)
        {
            publisher.Tick += new SoundPublisher.EventHandler(TakeAction);
        }

        private void TakeAction(SoundPublisher publisher, EventArgs e)
        {               
            publisher.SoundEffect.Play();
            
        }
    }
}
