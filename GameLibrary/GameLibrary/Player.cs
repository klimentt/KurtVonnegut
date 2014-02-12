using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Player : Agent
    {
        public int experience { get; private set; }
        public int mojo { get; private set; }
        public Inventory inventory { get; private set; }
    }
}
