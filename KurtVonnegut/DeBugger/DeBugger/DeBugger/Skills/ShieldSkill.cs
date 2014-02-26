namespace DeBugger
{
    using System;
    using Microsoft.Xna.Framework;

    public class ShieldSkill : Skill, ISkill
    {
        public ShieldSkill()
        {
        }

        TimeSpan Duration { get; set; }

        public override void Activate(GameTime time)
        {
            this.PreviousFireTime = time.TotalGameTime;
        }

        public void Initialize(Vector2 startPosition, Animation animation, TimeSpan cooldown, TimeSpan duration)
        {
            base.Initialize(startPosition, animation, cooldown);
            this.Duration = duration;
        }
    }
}
