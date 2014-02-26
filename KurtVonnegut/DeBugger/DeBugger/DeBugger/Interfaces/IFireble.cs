namespace DeBugger
{
    using System;

    public interface IFireble
    {
        TimeSpan FireTime { get; set; }
        TimeSpan PreviousFireTime { get; set; }
    }
}
