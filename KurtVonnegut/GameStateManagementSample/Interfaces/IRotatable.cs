﻿namespace DeBuggerGame
{
    using Microsoft.Xna.Framework;

    public interface IRotatable
    {
        float Rotation { get; set; }
        void RotateTowards(Vector2 position);
    }
}