using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    public interface IAggressive
    {
        bool IsInAggroRange { get; set; }
        float AggroRange { get; set; }
    }
}
