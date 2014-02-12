using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Armor : Item, IEquipable
    {
        public int critical { get; protected set; }
        public int health { get; protected set; }
        public int mojo { get; protected set; }
    }
}
