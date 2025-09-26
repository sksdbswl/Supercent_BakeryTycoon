using UnityEngine;

public interface IUnlockAction
{
    void Execute(Player player, UnlockTarget[] targets);
}

// UnlockContext는 사실 필요 없고, 대신 UnlockTarget 배열을 바로 전달하는 방식으로 변경
// 하지만 유지하고 싶다면 이렇게 확장 가능
public class UnlockContext
{
    public UnlockTarget[] Targets;
    public int Cost;
}