using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public abstract class Agent
    {
        protected int CoordX { get; set; }
        protected int CoordY { get; set; }
        protected int Health { get; set; }
    }
}
