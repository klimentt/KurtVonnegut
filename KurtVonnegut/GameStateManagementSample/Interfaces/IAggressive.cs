namespace DeBuggerGame
{
    public interface IAggressive
    {
        bool IsInAggroRange { get; set; }
        float AggroRange { get; set; }
    }
}
