using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public interface ISkill : IGameObject, IFireble
    {
        Animation Animation { get; set; }
        Vector2 StartPosition { get; set; }
        void Activate(IGameObject obj);
    }
}
