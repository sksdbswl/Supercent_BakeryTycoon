using UnityEngine;

public class Customer : MonoBehaviour
{
    public CustomerStateMachine CustomerStateMachine { get; private set; }
    public Animator animator;


    private void Awake()
    {
        // 초기 플레이어 설정
        animator = GetComponent<Animator>();

        // 초기 플레이어 생성 및 FSM 시작 선언
        CustomerStateMachine = new CustomerStateMachine(this);
        CustomerStateMachine.ChangeState(CustomerStateMachine.VisitState);
    }

    private void Start()
    {
    }

    private void Update()
    {
        CustomerStateMachine.Update();
    }
}