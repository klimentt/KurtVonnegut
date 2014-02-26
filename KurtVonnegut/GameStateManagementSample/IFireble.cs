using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeBuggerGame
{
    public interface IFireble
    {
        TimeSpan FireTime { get; set; }
        TimeSpan PreviousFireTime { get; set; }
    }
}
