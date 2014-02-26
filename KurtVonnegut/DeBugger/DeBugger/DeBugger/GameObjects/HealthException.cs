namespace DeBugger.GameObjects
{
    using System;


    public class HealthException<T> : Exception
    {
        public T Start { get; set; }
        public T End { get; set; }
        public HealthException(string msg, T start, T end)
            : base(msg)
        {
            this.Start = start;
            this.End = end;
        }
    }
}
