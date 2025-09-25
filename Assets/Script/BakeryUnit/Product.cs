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
    [SerializeField] private float moveDuration = 0.5f; // 이동 시간
    [SerializeField] private float jumpPower = 2f;     // 포물선 높이
    [SerializeField] private int jumpNum = 1;          // 점프 횟수

    
    public void MoveTo(Player player,Transform target, GoalType goalType)
    {
        switch (goalType)
        {
            case GoalType.Player:
            {
                player.PickedUpBreads.Push(this);
                
                int pickedUpBreads = player.PickedUpBreads.Count; 
                Vector3 lastBreadPos = target.position + new Vector3(0f, 0.5f * pickedUpBreads, 0f);
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

                        transform.localPosition = new Vector3(0f, 0.5f * pickedUpBreads, 0f);
                        transform.localRotation = Quaternion.identity;
                    });
                break;
            }
            case GoalType.Customer:
                //손님에게로 갈때 애니메이션
                break;
            case GoalType.Showcase:
                var showcase = target.GetComponent<Showcase>();

                // Showcase 스택에 미리 추가
                showcase.AddProduct(this);

                // 배치 계산
                int count = showcase.Products.Count - 1; // 현재 빵 인덱스
                int perRow = 6;  // 한 행에 6개
                int perLayer = 12; // 2행씩 한 층 -> 2x6 = 12
                int rowsPerLayer = 2;

                int layer = count / perLayer;                  // 층 계산
                int rowInLayer = (count % perLayer) / perRow; // 층 내 행
                int colInRow = count % perRow;                // 행 내 열

                float xStep = 0.5f;
                float yStep = 0.5f;
                float zStep = 0.5f;

                Vector3 startPos = player.BreadTransform.position; 
                if (player.PickedUpBreads.Count > 0)
                    startPos = player.PickedUpBreads.Peek().transform.position + target.right * -0.2f;

                transform.position = startPos;

                Vector3 targetPos = showcase.transform.position 
                                    + new Vector3(colInRow * xStep - (perRow - 1) * xStep / 2f, 
                                        layer * yStep, 
                                        rowInLayer * zStep - (rowsPerLayer - 1) * zStep / 2f);

                transform.DOJump(targetPos, jumpPower, jumpNum, moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        var rb = GetComponent<Rigidbody>();
                        if (rb != null) rb.isKinematic = true;

                        transform.SetParent(showcase.BreadPos);
                        transform.localPosition = new Vector3(colInRow * xStep - (perRow - 1) * xStep / 2f,
                            layer * yStep,
                            rowInLayer * zStep - (rowsPerLayer - 1) * zStep / 2f);
                        transform.localRotation = Quaternion.identity;
                    });

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(goalType), goalType, null);
        }
    }
}