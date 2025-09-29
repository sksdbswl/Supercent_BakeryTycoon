using UnityEngine;

public class OpenUnlockAction : IUnlockAction
{
    private ParticleSystem particle;

    public OpenUnlockAction(ParticleSystem particle)
    {
        this.particle = particle;
    }

    public void Execute(Player player, UnlockTarget[] targets)
    {
        foreach (var target in targets)
        {
            if (target == null) continue;

            if (target.Deactivate != null)
                target.Deactivate.SetActive(false);

            if (particle != null && target.Activate != null)
            {
                Transform particlePos = target.Activate.transform.Find("ParticlePos");
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
                    Debug.LogWarning("None particlePos on " + target.Activate.name);
                }
            }

            // 활성화 + 애니메이터 처리
            if (target.Activate != null)
            {
                target.Activate.SetActive(true);
                Animator anim = target.Activate.GetComponent<Animator>();
                if (anim != null)
                    anim.SetTrigger("Open");
            }

            if (target.CurrentUnlock != null)
            {
                target.CurrentUnlock.gameObject.SetActive(false);
            }

            if (target.NextUnlock != null)
            {
                target.NextUnlock.SetUnlockedState(false);
                target.NextUnlock.gameObject.SetActive(true);
            }
        }
    }
}