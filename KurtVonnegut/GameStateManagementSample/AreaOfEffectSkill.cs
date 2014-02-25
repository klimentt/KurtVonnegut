using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public class AreaOfEffectSkill : Skill, ISkill
    {
        public AreaOfEffectSkill(Vector2 startPosition, Animation animation, TimeSpan cooldown, float radius) : base(startPosition, animation, cooldown)
        {
            this.Radius = radius;
        }

        public float Radius { get; set; }

        public override void Activate(GameTime time)
        {
            this.PreviousFireTime = time.TotalGameTime;
        }
    }
}
