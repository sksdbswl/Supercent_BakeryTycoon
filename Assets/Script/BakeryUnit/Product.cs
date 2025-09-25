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
                // 1️⃣ 마지막 빵 위치 계산
                Vector3 lastBreadPos = target.position + new Vector3(0f, 0.5f * player.PickUpBread, 0f);

                // 2️⃣ 점프 시작 위치를 마지막 빵보다 앞쪽으로 오프셋
                float forwardOffset = 0.3f; // 앞쪽으로 0.3m
                Vector3 jumpStartPos = lastBreadPos - target.forward * forwardOffset; 
                transform.position = jumpStartPos;

                // 3️⃣ 포물선 이동 (목표 위치는 마지막 빵 위치)
                transform.DOJump(lastBreadPos, jumpPower, jumpNum, moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        // Rigidbody 끄기
                        var rg = gameObject.GetComponent<Rigidbody>();
                        if (rg != null) rg.isKinematic = true;

                        // 4️⃣ 부모 설정 및 방향
                        transform.SetParent(target);
                        transform.forward = player.transform.forward;

                        // 5️⃣ 최종 쌓기 위치
                        transform.localPosition = new Vector3(0f, 0.5f * player.PickUpBread, 0f);
                        transform.localRotation = Quaternion.identity;

                        // 6️⃣ 플레이어가 들고 있는 빵 수 증가
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