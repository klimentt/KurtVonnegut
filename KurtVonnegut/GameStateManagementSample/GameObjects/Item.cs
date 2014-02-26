namespace DeBuggerGame
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Item
        : AnimatedObject
    {
        #region fields

        protected int value;

        #endregion

        #region properties

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        #endregion

        #region constructors

        public Item()
            : base()
        {
        }

        #endregion
    }
}
