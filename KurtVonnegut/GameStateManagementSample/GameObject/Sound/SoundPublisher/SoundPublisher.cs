namespace GameStateManagementSample
{
    using System;
    using System.Threading;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;

    public class SoundPublisher : Sound
    {
        public event EventHandler Tick;

        public SoundEffect SoundEffect {get;set;}

        public delegate void EventHandler(SoundPublisher publisher, EventArgs e);

        public SoundPublisher(SoundEffect soundEffect)
            : base()
        {
            this.SoundEffect = soundEffect;
        }

        public void Execute()
        {
            if (Tick != null)
            {
                Tick(this, EventArgs.Empty);
            }

        }
    }
}
