using UnityEngine;

public static class CustomerAnimationController
{
    public static readonly int Idle = Animator.StringToHash("IDLE");
    public static readonly int Move  = Animator.StringToHash("Move");
    public static readonly int StackIdle  = Animator.StringToHash("StackIdle");
    public static readonly int StackMove  = Animator.StringToHash("StackMove");
}