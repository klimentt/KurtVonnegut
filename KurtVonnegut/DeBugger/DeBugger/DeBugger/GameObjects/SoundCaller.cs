namespace DeBugger
{
    using System;    
    using Microsoft.Xna.Framework.Audio;    

    public class SoundCaller : Sound
    {
        SoundPublisher publisher;
        SoundSubscriber subscriber;

        public SoundCaller(SoundEffect eff)
            : base()
        {
            publisher = new SoundPublisher(eff);
            subscriber = new SoundSubscriber();
            subscriber.Subscribe(publisher);
            publisher.Execute();
        }
    }
}
