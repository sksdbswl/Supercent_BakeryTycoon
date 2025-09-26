using UnityEngine;

public class UpgradeUnlockAction : IUnlockAction
{
    public void Execute(Player player, UnlockContext context)
    {
        Debug.Log("업그레이드 완료: 빵굽기 속도 + 쇼케이스 증가");
        // 업그레이드 로직
    }

    public void Execute(Player player, UnlockTarget[] targets)
    {
        throw new System.NotImplementedException();
    }
}