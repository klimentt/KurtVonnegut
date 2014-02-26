namespace DeBugger
{
    public class FireMoth
        : RotatingEnemy
    {
        public FireMoth()
            : base()
        {
            this.Value = 20;

            // activate enemy 
            this.Active = true;

            // set enemy health 
            this.Health = 60;

            // set enemy damage
            this.Damage = 20;

            // set enemy speed
            this.EnemyMoveSpeed = 0.5f;
        }
    }
}

