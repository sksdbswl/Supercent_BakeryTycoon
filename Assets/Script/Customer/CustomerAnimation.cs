using UnityEngine;

public static class CustomerAnimationController
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Move  = Animator.StringToHash("Move");
    public static readonly int StackIdle  = Animator.StringToHash("StackIdle");
    public static readonly int StackMove  = Animator.StringToHash("StackMove");
    public static readonly int Seat  = Animator.StringToHash("Seat");
    
    /// <summary>
    /// 등록된 모든 트리거 리셋
    /// </summary>
    public static void ResetAllTriggers(Animator animator)
    {
        if (animator == null) return;

        animator.ResetTrigger(Idle);
        animator.ResetTrigger(Move);
        animator.ResetTrigger(StackIdle);
        animator.ResetTrigger(StackMove);
        animator.ResetTrigger(Seat);
    }
}

