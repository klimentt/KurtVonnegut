namespace DeBuggerGame
{
    using Microsoft.Xna.Framework;

    public interface IGameObject
    {
        Vector2 Position { get; set; }
        int Width { get; }

        int Height { get; }
    }
}
