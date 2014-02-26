namespace GameStateManagementSample
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Weapon
        : Item
    {
        #region fields

        private int damage;

        #endregion

        #region properties

        public int Damage
        {
            get { return this.damage; }
            set
            {
                if (value <= 0)
                {
                    throw new System.ArgumentOutOfRangeException("Damage must be positive value!");
                }
                this.damage = value;
            }
        }

        #endregion

        #region constructor

        public Weapon()
            : base()
        {
        }


        #endregion
    }
}
