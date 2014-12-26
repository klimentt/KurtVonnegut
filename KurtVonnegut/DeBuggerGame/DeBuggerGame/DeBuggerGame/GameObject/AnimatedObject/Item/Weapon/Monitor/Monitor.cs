using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    public class Monitor
        : Weapon
    {
        #region constructor

        public Monitor()
            : base()
        {
            this.Active = true;
            this.Value = 5;
            this.Damage = 5;
        }

        #endregion
    }
}
