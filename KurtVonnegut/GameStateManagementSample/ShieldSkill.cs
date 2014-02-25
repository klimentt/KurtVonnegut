using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public class ShieldSkill : Skill, ISkill
    {
        public ShieldSkill(TimeSpan cooldown, TimeSpan duration, Animation animation, IGameObject origin) : base(origin.Position, animation, cooldown)
        {
            this.Duration = duration;
        }

        TimeSpan Duration { get; set; }

        public override void Activate(GameTime time)
        {
            this.PreviousFireTime = time.TotalGameTime;
        }
    }
}
