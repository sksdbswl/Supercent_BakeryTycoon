using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        if (player == null) return;
        if (!isManualMoving)
            transform.position = player.transform.position + offset;
    }

    private bool isManualMoving = false;

    /// <summary>
    /// 카메라를 특정 위치로 이동했다가 다시 플레이어 위치로 복귀
    /// </summary>
    public void MoveToPositionTemporarily(Vector3 targetPos, float moveDuration, float stayDuration)
    {
        if (isManualMoving) return;

        StartCoroutine(MoveRoutine(targetPos, moveDuration, stayDuration));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, float moveDuration, float stayDuration, float zOffset = -9f, float yOffset = 2f)
    {
        isManualMoving = true;

        targetPos.z += zOffset;
        targetPos.y += yOffset;

        yield return transform.DOMove(targetPos, moveDuration).SetEase(Ease.InOutSine).WaitForCompletion();

        yield return new WaitForSeconds(stayDuration);

        Vector3 playerPos = player.transform.position + offset;
        yield return transform.DOMove(playerPos, moveDuration).SetEase(Ease.InOutSine).WaitForCompletion();

        isManualMoving = false;
    }
}