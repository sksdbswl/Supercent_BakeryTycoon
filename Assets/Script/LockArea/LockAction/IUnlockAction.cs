using UnityEngine;

public interface IUnlockAction
{
    void Execute(Player player, UnlockTarget[] targets);
}

public class UnlockContext
{
    public UnlockTarget[] Targets;
    public int Cost;
}