namespace GameStateManagementSample
{
    using System;
    using System.Threading;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class SoundCaller : Sound
    {
        SoundPublisher publisher;
        SoundSubscriber subscriber;
        
        public SoundCaller(SoundEffect eff)
            :base()
        {
            publisher = new SoundPublisher(eff);
            subscriber = new SoundSubscriber();
            subscriber.Subscribe(publisher);
            publisher.Execute();
        }

        
    }
}
