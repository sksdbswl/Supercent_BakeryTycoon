using System;
using UnityEngine;

public class VirtualJoystickCtrl : MonoBehaviour
{
    [Header("JoyController")]
    [SerializeField] private RectTransform JoysticBackground;
    [SerializeField] private RectTransform JoysticHandle;
    [SerializeField] private RectTransform ExtraIndicator;
    [Space]
    [SerializeField] private float handleSpeed = 10f;

    private float moveRange;
    private Vector3 startPos;
    private Vector2 moveInput;

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

        if (ExtraIndicator != null)
            ExtraIndicator.gameObject.SetActive(true);

        JoysticBackground.position = startPos;
        JoysticHandle.position = startPos;
        if (ExtraIndicator != null) ExtraIndicator.position = startPos;

        moveRange = JoysticBackground.sizeDelta.x * 0.3f;
    }

    private void DragStick()
    {
        Vector2 rawDirection = (Vector2)Input.mousePosition - (Vector2)startPos;
        Vector2 clampedDirection = Vector2.ClampMagnitude(rawDirection, moveRange);

        JoysticHandle.position = Vector3.Lerp(
            JoysticHandle.position,
            startPos + (Vector3)clampedDirection * 2f,
            Time.deltaTime * handleSpeed
        );

        if (ExtraIndicator != null)
            ExtraIndicator.position = Input.mousePosition;

        moveInput = clampedDirection / moveRange;
    }

    private void EndStick()
    {
        JoysticBackground.gameObject.SetActive(false);

        if (ExtraIndicator != null)
            ExtraIndicator.gameObject.SetActive(false);

        moveInput = Vector2.zero;
    }
}
