using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    public interface IGameObject
    {
        Vector2 Position { get; set; }
        int Width { get; }

        int Height { get; }
    }
}
