namespace GameStateManagementSample
{
    public class Gem
        : Currency
    {
        #region constructors

        public Gem()
            : base()
        {
            this.Value = 20;
            this.Active = true;
        }

        #endregion
    }
}
