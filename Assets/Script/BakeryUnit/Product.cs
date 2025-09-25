using System.Collections;
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

    public void Init(BreadSpawner spawner)
    {
        originSpawner = spawner;
    }
    
    public void MoveTo(Player player, Transform target, GoalType goalType)
    {
        switch (goalType)
        {
            case GoalType.Player:
                originSpawner.PickupBread();
                StartCoroutine(MoveToPlayerBezier(player, target));
                break;

            case GoalType.Customer:
                // 손님에게 이동 구현 예정
                break;

            case GoalType.Showcase:
                StartCoroutine(MoveToShowcaseBezier(player, target));
                break;
        }
    }

    private IEnumerator MoveToPlayerBezier(Player player, Transform target)
    {
        player.PickedUpBreads.Push(this);

        int pickedUpBreads = player.PickedUpBreads.Count;
        Vector3 endPos = target.position + new Vector3(0f, 0.5f * pickedUpBreads, 0f);
        float sideOffset = -0.8f;

        Vector3 startPos = endPos - target.right * sideOffset;
        transform.position = startPos;
        transform.rotation = player.transform.rotation * Quaternion.Euler(0f, -90f, 0f);

        // 제어점 2개 (중간에서 위로)
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
        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        transform.SetParent(target);
        transform.localPosition = new Vector3(0f, 0.5f * pickedUpBreads, 0f);
        transform.localRotation = Quaternion.identity;
    }

    private IEnumerator MoveToShowcaseBezier(Player player, Transform target)
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
        float yStep = 0.2f;
        float zStep = 0.8f;

        Vector3 startPos = player.PickedUpBreads.Count > 0
            ? player.PickedUpBreads.Peek().transform.position + target.right * -0.2f
            : player.BreadTransform.position;

        Vector3 endPos = showcase.transform.position +
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
        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        transform.SetParent(showcase.BreadPos);
        transform.localPosition = new Vector3(
            colInRow * xStep - (perRow - 1) * xStep / 2f,
            layer * yStep,
            rowInLayer * zStep - (rowsPerLayer - 1) * zStep / 2f);
        transform.localRotation = Quaternion.identity;
    }
}
