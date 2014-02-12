using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Weapon : Item, IEquipable
    {
        public int damage { get; protected set; }
        public int critical { get; protected set; }
        public int health { get; protected set; }
        public int mojo { get; protected set; }
    }
}
