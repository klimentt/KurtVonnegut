namespace DeBugger
{
    public interface IAggressive
    {
        bool IsInAggroRange { get; set; }
        float AggroRange { get; set; }
    }
}
