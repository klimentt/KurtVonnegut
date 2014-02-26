﻿namespace DeBugger
{
    public class Coin
        : Currency
    {
        #region constructors

        public Coin()
            : base()
        {
            this.Value = 1;
            this.Active = true;
        }

        #endregion
    }
}