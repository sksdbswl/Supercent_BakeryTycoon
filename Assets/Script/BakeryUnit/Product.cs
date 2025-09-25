using System;
using DG.Tweening;
using UnityEngine;

public class Product : MonoBehaviour
{
    public enum GoalType
    {
        Player,
        Customer,
        Showcase,
    }
    
    [Header("Move Settings")]
    [SerializeField] private float moveDuration = 1f; // 이동 시간
    [SerializeField] private float jumpPower = 2f;     // 포물선 높이
    [SerializeField] private int jumpNum = 1;          // 점프 횟수

    public void MoveTo(Player player,Transform target, GoalType goalType)
    {
        switch (goalType)
        {
            case GoalType.Player:
            {
                Vector3 lastBreadPos = target.position + new Vector3(0f, 0.5f * player.PickUpBread, 0f);

                // float forwardOffset = 0.3f; 
                // Vector3 jumpStartPos = lastBreadPos - target.forward * forwardOffset;
                // transform.position = jumpStartPos;
                
                // 2️⃣ 점프 시작 위치: x축 방향 offset
                float sideOffset = -0.8f; 
                Vector3 jumpStartPos = lastBreadPos - target.right * sideOffset;
                transform.position = jumpStartPos;


                transform.rotation = player.transform.rotation * Quaternion.Euler(0f, -90f, 0f);

                transform.DOJump(lastBreadPos, jumpPower, jumpNum, moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        var rg = gameObject.GetComponent<Rigidbody>();
                        if (rg != null) rg.isKinematic = true;

                        transform.SetParent(target);

                        transform.localPosition = new Vector3(0f, 0.5f * player.PickUpBread, 0f);
                        transform.localRotation = Quaternion.identity;

                        player.PickUpBread++;
                    });
            }
                break;

             // transform.DOJump(target.position, jumpPower, jumpNum, moveDuration)
               //      .SetEase(Ease.OutQuad)
               //      .OnComplete(() =>
               //      {
               //          var rg = gameObject.GetComponent<Rigidbody>();
               //          if (rg != null) rg.isKinematic = true;
               //
               //          transform.SetParent(target);
               //          transform.forward = player.transform.forward;
               //
               //          transform.localPosition = new Vector3(0f, 0.5f * player.PickUpBread, 0f);
               //          transform.localRotation = Quaternion.identity;
               //      });
            case GoalType.Customer:
                //손님에게로 갈때 애니메이션
                break;
            case GoalType.Showcase:
                //쇼케이스 갈때 애니메이션
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(goalType), goalType, null);
        }
    }
}