using System;
using UnityEngine;

public class VirtualJoystickCtrl : MonoBehaviour
{
    [Header("JoyController")]
    [SerializeField] private RectTransform JoysticBackground;
    [SerializeField] private RectTransform JoysticHandle;
    [Space]
    [SerializeField] private float handleSpeed = 10f; // 기본값 설정

    private float moveRange;      // 조이스틱 이동 범위
    private Vector3 startPos;     // 조이스틱 시작 위치
    private Vector2 moveInput;    // 정규화된 입력값 ( -1 ~ 1 범위 )

    public Vector2 MoveInput => moveInput;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartStick();

        if (Input.GetMouseButton(0))
            DragStick();

        if (Input.GetMouseButtonUp(0))
            EndStick();
    }

    private void StartStick()
    {
        startPos = Input.mousePosition;
        JoysticBackground.gameObject.SetActive(true);

        JoysticBackground.position = startPos;
        JoysticHandle.position = startPos;

        moveRange = JoysticBackground.sizeDelta.x * 0.3f;
    }

    private void DragStick()
    {
        Vector2 direction = (Vector2)Input.mousePosition - (Vector2)startPos;

        // 최대 moveRange까지만 이동
        direction = Vector2.ClampMagnitude(direction, moveRange);

        // 핸들 위치 보간 이동
        JoysticHandle.position = Vector3.Lerp(
            JoysticHandle.position,
            startPos + (Vector3)direction,
            Time.deltaTime * handleSpeed
        );

        // MoveInput은 정규화된 값 (-1 ~ 1)
        moveInput = direction / moveRange;
    }

    private void EndStick()
    {
        JoysticBackground.gameObject.SetActive(false);
        moveInput = Vector2.zero;
    }
}