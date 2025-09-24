using UnityEngine;

public class OpenUnlockAction : IUnlockAction
{
    public void Execute(Player player, UnlockContext context)
    {
        Debug.Log("새로운 자리가 열렸습니다!");
        
        if (context.Target != null)
        {
            if(context.NoneTarget != null) context.NoneTarget.SetActive(false);
            
            context.Target.SetActive(true);
            Animator anim = context.Target.GetComponent<Animator>();
            if(anim != null) anim.SetTrigger("Open");
        }
    }
}

