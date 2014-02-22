using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagementSample
{
    public interface IAggressive
    {
        bool IsInAggroRange { get; set; }
        float AggroRange { get; set; }
    }
}
