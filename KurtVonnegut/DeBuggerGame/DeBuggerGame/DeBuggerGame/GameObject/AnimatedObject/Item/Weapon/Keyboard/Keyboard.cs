using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    public class Keyboard
        : Weapon
    {
        #region constructor

        public Keyboard()
            : base()
        {
            this.Active = true;
            this.Value = 2;
            this.Damage = 3;
        }

        #endregion
    }
}
