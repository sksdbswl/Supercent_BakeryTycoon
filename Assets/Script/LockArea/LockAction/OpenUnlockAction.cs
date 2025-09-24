using UnityEngine;

public class OpenUnlockAction : IUnlockAction
{
    private ParticleSystem particle;

    public OpenUnlockAction(ParticleSystem particle)
    {
        this.particle = particle;
    }
    
    public void Execute(Player player, UnlockContext context)
    {
        Debug.Log("새로운 자리가 열렸습니다!");
        
        if (context.Target != null)
        {
            if(context.NoneTarget != null) context.NoneTarget.SetActive(false);
            
            Transform particlePos = context.Target.transform.Find("ParticlePos");

            if (particlePos != null)
            {
                ParticleSystem p = GameObject.Instantiate(
                    particle, 
                    particlePos.position, 
                    particlePos.rotation, 
                    particlePos
                );

                p.Play();
            }
            else
            {
                Debug.LogWarning("None particlePos");
            }
            
            context.Target.SetActive(true);
            Animator anim = context.Target.GetComponent<Animator>();
            if(anim != null) anim.SetTrigger("Open");
        }
    }
}

