using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace DeBuggerGame
{
    public interface IRotatable
    {
        float Rotation { get; set; }
        void RotateTowards(Vector2 position);
    }
}