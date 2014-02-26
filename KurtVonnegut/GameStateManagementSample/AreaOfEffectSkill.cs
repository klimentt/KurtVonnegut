using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public class AreaOfEffectSkill : Skill, ISkill
    {
        public AreaOfEffectSkill()
        {
        }

        public float Radius { get; set; }

        public override void Activate(GameTime time)
        {
            this.PreviousFireTime = time.TotalGameTime;
        }

        public void Initialize(Vector2 startPosition, Animation animation, TimeSpan cooldown, float radius)
        {
            base.Initialize(startPosition, animation, cooldown);

            this.Radius = radius;
        }
    }
}
