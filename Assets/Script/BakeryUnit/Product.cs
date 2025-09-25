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
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private float curveHeight = 2f; // 배지어 제어점 높이

    public void MoveTo(Player player, Transform target, GoalType goalType)
    {
        switch (goalType)
        {
            case GoalType.Player:
                MoveToPlayer(player, target);
                break;

            case GoalType.Customer:
                // 손님에게 이동 구현
                break;

            case GoalType.Showcase:
                MoveToShowcase(player, target);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(goalType), goalType, null);
        }
    }

    private void MoveToPlayer(Player player, Transform target)
    {
        player.PickedUpBreads.Push(this);

        int pickedUpBreads = player.PickedUpBreads.Count;
        Vector3 endPos = target.position + new Vector3(0f, 0.5f * pickedUpBreads, 0f);
        float sideOffset = -0.8f;

        Vector3 startPos = endPos - target.right * sideOffset;
        transform.position = startPos;
        transform.rotation = player.transform.rotation * Quaternion.Euler(0f, -90f, 0f);

        // 제어점: 시작점과 끝점 사이 높이를 올려서 곡선 생성
        Vector3 controlPoint = (startPos + endPos) / 2f + Vector3.up * curveHeight;

        Vector3[] path = { startPos, controlPoint, endPos };

        transform.DOPath(path, moveDuration, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                var rb = GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;

                transform.SetParent(target);
                transform.localPosition = new Vector3(0f, 0.5f * pickedUpBreads, 0f);
                transform.localRotation = Quaternion.identity;
            });
    }

    private void MoveToShowcase(Player player, Transform target)
    {
        var showcase = target.GetComponent<Showcase>();
        showcase.AddProduct(this);

        int count = showcase.Products.Count - 1;
        int perRow = 10;
        int perLayer = 20;
        int rowsPerLayer = 2;

        int layer = count / perLayer;
        int rowInLayer = (count % perLayer) / perRow;
        int colInRow = count % perRow;

        float xStep = 0.2f;
        float yStep = 1f;
        float zStep = 0.8f;

        Vector3 startPos = player.PickedUpBreads.Count > 0
            ? player.PickedUpBreads.Peek().transform.position + target.right * -0.2f
            : player.BreadTransform.position;

        transform.position = startPos;

        Vector3 endPos = showcase.transform.position 
                         + new Vector3(colInRow * xStep - (perRow - 1) * xStep / 2f, 
                             layer * yStep, 
                             rowInLayer * zStep - (rowsPerLayer - 1) * zStep / 2f);

        Vector3 controlPoint = (startPos + endPos) / 2f + Vector3.up * curveHeight;

        Vector3[] path = { startPos, controlPoint, endPos };

        transform.DOPath(path, moveDuration, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                var rb = GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;

                transform.SetParent(showcase.BreadPos);
                transform.localPosition = new Vector3(
                    colInRow * xStep - (perRow - 1) * xStep / 2f,
                    layer * yStep,
                    rowInLayer * zStep - (rowsPerLayer - 1) * zStep / 2f);
                transform.localRotation = Quaternion.identity;
            });
    }
}
