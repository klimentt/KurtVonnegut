namespace DeBugger
{
    /// <summary>
    /// Within hit detection this enemy will colide but not take damage from solids, other enemies and the player
    /// </summary>
    public class WalkingEnemy : RotatingEnemy, IGameObject, IRotatable, IAggressive
    {
    }
}