using UnityEngine;

public class OpenUnlockAction : IUnlockAction
{
    private ParticleSystem particle;

    public OpenUnlockAction(ParticleSystem particle)
    {
        this.particle = particle;
    }

    // UnlockTarget 배열로 수정
    public void Execute(Player player, UnlockTarget[] targets)
    {
        Debug.Log("새로운 자리가 열렸습니다!");

        foreach (var target in targets)
        {
            if (target == null) continue;

            // 비활성화할 오브젝트 처리
            if (target.Deactivate != null)
                target.Deactivate.SetActive(false);

            // 파티클 재생
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

            target.CurrentUnlock.gameObject.SetActive(false);

            // 다음 해금 지역 초기화 (잠금 해제 가능 상태)
            if (target.NextUnlock != null)
            {
                target.NextUnlock.SetUnlockedState(false);
                target.NextUnlock.gameObject.SetActive(true);
            }
        }
    }
}