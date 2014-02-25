﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public class ShieldSkill : Skill, ISkill
    {
        public ShieldSkill(TimeSpan cooldown, TimeSpan duration, Animation animation, Vector2 position) : base(position, animation, cooldown)
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
