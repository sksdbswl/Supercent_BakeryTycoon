using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    public Animator animator;


    private void Awake()
    {
        // 초기 플레이어 설정
        animator = GetComponentInChildren<Animator>();

        // 초기 플레이어 생성 및 FSM 시작 선언
        PlayerStateMachine = new PlayerStateMachine(this);
        PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);
    }

    private void Start()
    {
    }

    private void Update()
    {
        PlayerStateMachine.Update();
    }
}