using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VirtualJoystickCtrl : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public float moveRange = 50f;

    [HideInInspector]
    public Vector2 MoveInput;

    private Vector2 joystickCenter;

    private void Start() => joystickCenter = joystickBackground.position;

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickCenter;
        direction = Vector2.ClampMagnitude(direction, moveRange);
        joystickHandle.position = joystickCenter + direction;
        MoveInput = direction / moveRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickHandle.position = joystickCenter;
        MoveInput = Vector2.zero;
    }
}