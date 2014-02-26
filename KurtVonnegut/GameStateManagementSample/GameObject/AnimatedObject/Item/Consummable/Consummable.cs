namespace GameStateManagementSample
{
    public abstract class Consummable
        : Item
    {
        #region fields

        protected int heal;

        #endregion

        #region properties

        public int Heal
        {
            get { return this.heal; }
            set
            {
                if (value <= 0)
                {
                    throw new System.ArgumentOutOfRangeException("Heal must be positive value!");
                }
                this.heal = value;
            }
        }

        #endregion

        #region constructors

        public Consummable()
            : base()
        {
        }

        #endregion
    }
}
