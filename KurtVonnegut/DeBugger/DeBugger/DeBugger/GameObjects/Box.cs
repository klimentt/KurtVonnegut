namespace DeBugger
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// This is an object that can be moved by the player
    /// </summary>
    public class Box : Solid
    {
        public void MoveByAmount(Vector2 direction)
        {
            this.Position += direction;
        }
    }
}