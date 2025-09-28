using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    private Player player;
    
    [Header("Settings")]
    public float moveSpeed = 5f;

    [Header("References")]
    public VirtualJoystickCtrl joystick; // 모바일 조이스틱
    private CharacterController controller;
    private Camera mainCam;

    private Vector2 moveInput; // 키보드/게임패드
    private Vector3 clickTarget;
    private bool isClickMoving = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;
    }

    #region Input System 이벤트
    // 키보드/게임패드 이동
    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("움직이는 중");
        moveInput = context.ReadValue<Vector2>();
    }

    // 마우스 클릭 이동
    public void OnClickMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                clickTarget = hit.point;
                isClickMoving = true;
            }
        }
    }
    #endregion
    /// <summary>
    /// 현재 입력으로부터 이동 방향을 계산
    /// </summary>
    public Vector3 GetMoveDirection()
    {
        Vector3 move = Vector3.zero;

        // 1. 모바일 조이스틱
        if (joystick != null && joystick.MoveInput.sqrMagnitude > 0.01f)
        {
            move = new Vector3(joystick.MoveInput.x, 0, joystick.MoveInput.y);
            isClickMoving = false;
        }
        // 2. 키보드/게임패드
        else if (moveInput.sqrMagnitude > 0.01f)
        {
            move = new Vector3(moveInput.x, 0, moveInput.y);
            isClickMoving = false;
        }
        // 3. 마우스 클릭 이동
        else if (isClickMoving)
        {
            Vector3 dir = clickTarget - transform.position;
            dir.y = 0;
            if (dir.magnitude < 0.1f)
                isClickMoving = false;
            else
                move = dir.normalized;
        }

        return move;
    }
    
    /// <summary>
    /// 실제 이동 처리
    /// </summary>
    public void Move(Vector3 move)
    {
        if (move.sqrMagnitude > 0.01f)
        {
            controller.Move(move * (moveSpeed * Time.deltaTime));
            transform.forward = move.normalized;
        }
    }
}
