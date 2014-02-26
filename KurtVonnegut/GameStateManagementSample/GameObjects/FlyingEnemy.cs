﻿namespace DeBuggerGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This is an enemy that in hit detection should fly over solid objects, should colide but not take damage from players and other enemies.
    /// </summary>
    public class FlyingEnemy : RotatingEnemy, IGameObject, IRotatable, IAggressive
    {
    }
}
