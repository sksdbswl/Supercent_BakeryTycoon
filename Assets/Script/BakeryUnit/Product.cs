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
        Box
    }

    [Header("Move Settings")]
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private float curveHeight = 2f;
    private BreadSpawner originSpawner;
    private Showcase originShowcase;
    
    public PooledObject PooledObject { get; set; }
    
    public void Init(BreadSpawner spawner, Showcase showcase)
    {
        PooledObject =GetComponent<PooledObject>();
        originSpawner = spawner;
        originShowcase = showcase;
    }

    // Player/Customer/Showcase/Box 통합 이동
    public void MoveTo(IProductTarget target, GoalType goalType)
    {
        switch (goalType)
        {
            case GoalType.Player:
                // 오븐에서 -> 플레이어 ( product -> IProductTarget )
                originSpawner.PickupBread();
                StartCoroutine(MoveToTargetBezier(target));
                break;

            case GoalType.Showcase:
                // 플레이어 -> 쇼케이스 ( product -> showcase )
                StartCoroutine(MoveToShowcaseBezier());
                break;
            
            case GoalType.Customer:
                // 쇼케이스 -> 손님 ( showcase -> product )
                StartCoroutine(MoveToTargetBezier(target));
                break;
            
            case GoalType.Box:
                // 손님 -> Box ( product -> product )
                StartCoroutine(MoveToBoxBezier(target));
                break;
        }
    }
    
    private IEnumerator MoveToBoxBezier(IProductTarget target)
    {
        // 손님의 빵 (this) -> 박스로 이동
        //target.PickedUpBreads.Push(this);
        
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

    // 오븐,쇼케이스 -> Player / Customer 픽업지점 :: 공용 Bezier 이동
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
    public IEnumerator MoveToShowcaseBezier()
    {
        int count = originShowcase.Products.Count - 1 ;

        // 배치 규칙
        int perRow = 5;   // 가로 2
        int perCol = 2;   // 세로 5
        int perLayer = perRow * perCol; // 한 층에 10개

        // 현재 인덱스에 따라 위치 계산
        int layer = count / perLayer;               // 몇 번째 층인지
        int rowInLayer = (count % perLayer) / perRow; // 세로 줄
        int colInRow = count % perRow;              // 가로 칸

        // 스텝 크기
        float xStep = 0.4f;
        float yStep = 0.3f;
        float zStep = 0.7f;

        Vector3 startPos = transform.position;
        Vector3 endPos = originShowcase.transform.position +
                         new Vector3(
                             colInRow * xStep - (perRow - 1) * xStep / 2f,
                             layer * yStep,
                             rowInLayer * zStep - (perCol - 1) * zStep / 2f
                         );

        // 제어점 (단순히 중간 + 위로 curveHeight 올림)
        Vector3 control1 = (startPos + endPos) / 2f + Vector3.up * curveHeight;
        Vector3 control2 = control1; 

        // 곡선 이동
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            transform.position = Bezier.Cubic(startPos, control1, control2, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 부모 변경 + 로컬 정렬
        transform.SetParent(originShowcase.BreadPos);
        transform.localPosition = new Vector3(
            colInRow * xStep - (perRow - 1) * xStep / 2f,
            layer * yStep,
            rowInLayer * zStep - (perCol - 1) * zStep / 2f
        );
        
        transform.localRotation = Quaternion.identity;
    }
}

