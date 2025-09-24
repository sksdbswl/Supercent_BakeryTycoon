using UnityEngine;

public interface IUnlockAction
{
    void Execute(Player player, UnlockContext context);
}

public class UnlockContext
{
    public GameObject Target;
    public GameObject NoneTarget;
    public int Cost;
}
