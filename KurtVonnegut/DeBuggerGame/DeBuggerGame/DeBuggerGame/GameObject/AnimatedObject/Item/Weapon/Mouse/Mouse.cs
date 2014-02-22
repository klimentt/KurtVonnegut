using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    public class Mouse
        : Weapon
    {
        #region constructor

        public Mouse()
            : base()
        {
            this.Active = true;
            this.Value = 1;
            this.Damage = 2;
        }

        #endregion
    }
}
