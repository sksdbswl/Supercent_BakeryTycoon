using UnityEngine;

public class EmploymentUnlockAction : IUnlockAction
{
    public void Execute(Player player, UnlockContext context)
    {
        Debug.Log("새로운 직원이 고용되었습니다!");
        // 직원 스폰 로직
    }

    public void Execute(Player player, UnlockTarget[] targets)
    {
        throw new System.NotImplementedException();
    }
}