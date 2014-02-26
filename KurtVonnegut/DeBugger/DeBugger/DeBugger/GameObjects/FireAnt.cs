namespace DeBugger
{
    public class FireAnt
        : RotatingEnemy
    {
        public FireAnt()
            : base()
        {
            this.Value = 5;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 8;

            // set enemy damage
            this.Damage = 15;

            // set enemy speed
            this.EnemyMoveSpeed = 0.8f;
        }
    }
}
