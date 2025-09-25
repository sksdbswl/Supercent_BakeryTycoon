using System.Collections;
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
    [SerializeField] private float curveHeight = 2f;
    private BreadSpawner originSpawner;
    private Showcase originShowcase;
    
    public void Init(BreadSpawner spawner, Showcase showcase)
    {
        originSpawner = spawner;
        originShowcase = showcase;
    }

    // Player/Customer/Showcase 통합 이동
    public void MoveTo(IProductTarget target, GoalType goalType)
    {
        switch (goalType)
        {
            case GoalType.Player:
                // 오븐에서 -> 플레이어
                originSpawner.PickupBread();
                StartCoroutine(MoveToTargetBezier(target));
                break;

            case GoalType.Customer:
                // 쇼케이스 -> 손님
                StartCoroutine(MoveToTargetBezier(target));
                break;

            case GoalType.Showcase:
                // 플레이어 -> 쇼케이스
                StartCoroutine(MoveToShowcaseBezier(target));
                break;
        }
    }

    // Player / Customer 공용 Bezier 이동
    private IEnumerator MoveToTargetBezier(IProductTarget target)
    {
        target.PickedUpBreads.Push(this);
        
        int count = target.PickedUpBreads.Count;
        
        Vector3 startPos = transform.position;
        Vector3 endPos = target.BreadTransform.position + new Vector3(0f, 0.5f * count, 0f);

        Vector3 control1 = startPos + Vector3.up * curveHeight;
        Vector3 control2 = endPos + Vector3.up * curveHeight;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            transform.position = Bezier.Cubic(startPos, control1, control2, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        transform.SetParent(target.BreadTransform);
        transform.localPosition = new Vector3(0f, 0.5f * count, 0f);
        transform.localRotation = Quaternion.identity;
    }

    [Header("Movement Settings")]
    public float rotate = 180f; // 목표 회전
    public float xStep = 0.5f;  // 가로 간격
    public float yStep = 0.3f;  // 레이어 높이
    public float zStep = 0.5f;  // 세로 간격
    
    // player -> Showcase 이동
    public IEnumerator MoveToShowcaseBezier(IProductTarget target)
    {
        //TODO:: 쇼케이스 위치로 생성 
        
        int count = originShowcase.Products.Count;
        int perRow = 10;
        int perLayer = 20;
        int rowsPerLayer = 2;

        int layer = count / perLayer;
        int rowInLayer = (count % perLayer) / perRow;
        int colInRow = count % perRow;

        float xStep = 0.2f;
        float yStep = 1f;
        float zStep = 0.8f;

        Vector3 startPos = transform.position;
        Vector3 endPos = originShowcase.transform.position +
                         new Vector3(colInRow * xStep - (perRow - 1) * xStep / 2f,
                             layer * yStep,
                             rowInLayer * zStep - (rowsPerLayer - 1) * zStep / 2f);

        // 제어점 2개
        Vector3 control1 = (startPos + endPos) / 2f + Vector3.up * curveHeight;
        Vector3 control2 = control1; 

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            transform.position = Bezier.Cubic(startPos, control1, control2, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        
        // var rb = GetComponent<Rigidbody>();
        // if (rb != null) rb.isKinematic = true;
        //
        // Vector3 endPos = originShowcase.transform.position 
        //                  + new Vector3(colInRow * xStep - (perRow - 1) * xStep / 2f, 
        //                      layer * yStep, 
        //                      rowInLayer * zStep - (rowsPerLayer - 1) * zStep / 2f);
        //
        // Vector3 controlPoint = (startPos + endPos) / 2f + Vector3.up * curveHeight;
        //
        // Vector3[] path = { startPos, controlPoint, endPos };
        //
        // transform.DOPath(path, moveDuration, PathType.CatmullRom)
        //     .SetEase(Ease.OutQuad)
        //     .OnComplete(() =>
        //     {
        //         var rb = GetComponent<Rigidbody>();
        //         if (rb != null) rb.isKinematic = true;
        //
        //         transform.SetParent(originShowcase.BreadPos);
        //         transform.localPosition = new Vector3(
        //             colInRow * xStep - (perRow - 1) * xStep / 2f,
        //             layer * yStep,
        //             rowInLayer * zStep - (rowsPerLayer - 1) * zStep / 2f);
        //         transform.localRotation = Quaternion.identity;
        //     });
        // transform.SetParent(originShowcase.BreadPos);
        // transform.localPosition = new Vector3(
        //     colInRow * xStep - (perRow - 1) * xStep / 2f,
        //     layer * yStep,
        //     rowInLayer * zStep - (rowsPerLayer - 1) * zStep / 2f);
        // transform.localRotation = Quaternion.identity;
    
        // if (originShowcase == null)
        // {
        //     Debug.LogError("MoveToShowcaseBezier: originShowcase is null!");
        //     yield break;
        // }
        //
        // Vector3 startPos = transform.position;
        // int count = originShowcase.Products.Count;
        //
        // int perRow = 2;
        // int perColumn = 5;
        //
        // int row = count % perRow;
        // int column = (count / perRow) % perColumn;
        // int layer = count / (perRow * perColumn);
        //
        // Vector3 localTargetPos = new Vector3(
        //     row * xStep - (perRow - 1) * xStep / 2f,
        //     layer * yStep,
        //     column * zStep - (perColumn - 1) * zStep / 2f
        // );
        //
        // Vector3 endPos = originShowcase.BreadPos.position + localTargetPos;
        //
        // Vector3 control1 = startPos + Vector3.up * curveHeight;
        // Vector3 control2 = endPos + Vector3.up * curveHeight;
        //
        // float elapsed = 0f;
        // while (elapsed < moveDuration)
        // {
        //     float t = elapsed / moveDuration;
        //     transform.position = Bezier.Cubic(startPos, control1, control2, endPos, t);
        //     elapsed += Time.deltaTime;
        //     yield return null;
        // }
        //
        // transform.position = endPos;
        //
        // transform.SetParent(originShowcase.BreadPos);
        // transform.localPosition = localTargetPos;
        // transform.localRotation = Quaternion.identity;
        //
        // originShowcase.Exhibition(this);
        // yield return null;
    }

}

