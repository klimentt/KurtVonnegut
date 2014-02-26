using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    /// <summary>
    /// This is an object that can be moved by the player
    /// </summary>
    public class Box : Solid
    {
        public void MoveByAmount(Vector2 direction)
        {
            this.Position += direction;
        }
    }
}
