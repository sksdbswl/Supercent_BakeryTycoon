using UnityEngine;

public class Customer : MonoBehaviour
{
    public CustomerStateMachine CustomerStateMachine { get; private set; }
    public Animator animator;

    public CustomerData customerData;   // SO 데이터
    
    private void Awake()
    {
        // 초기 플레이어 설정
        animator = GetComponent<Animator>();

        // 초기 플레이어 생성 및 FSM 시작 선언
        CustomerStateMachine = new CustomerStateMachine(this);
        CustomerStateMachine.ChangeState(CustomerStateMachine.VisitState);
        
        // 주문 초기화
        InitCustomer();
    }
    
    public void InitCustomer()
    {
        customerData = GameManager.Instance.CustomerTable.GetRandomCustomer();
    }

    private void Update()
    {
        CustomerStateMachine.Update();
    }
}