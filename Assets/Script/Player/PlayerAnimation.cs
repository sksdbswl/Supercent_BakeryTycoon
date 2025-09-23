using UnityEngine;

public static class PlayerAnimationController
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Move  = Animator.StringToHash("Move");
    public static readonly int StackIdle  = Animator.StringToHash("StackIdle");
    public static readonly int StackMove  = Animator.StringToHash("StackMove");
}