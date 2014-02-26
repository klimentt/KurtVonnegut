using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    /// <summary>
    /// Within hit detection this enemy will colide but not take damage from solids, other enemies and the player
    /// </summary>
    public class WalkingEnemy : RotatingEnemy, IGameObject, IRotatable, IAggressive
    {
    }
}
