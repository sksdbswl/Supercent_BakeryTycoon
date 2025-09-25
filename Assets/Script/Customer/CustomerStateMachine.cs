
public class CustomerStateMachine : StateMachine
{
    /// <summary>
    /// 상태(State) 예시
    /// VisitState : 방문 상태 ( 걸어오는 모션, 쇼케이스 이동 및 빵 픽업 )
    /// OrderWaiting: 계산 대기 ( 해당 조건에 맞는 위치로 전환 )
    /// BuyState: 계산 및 포장 -> Leaving: 매장 떠남
    /// EattingState : 식사 위치로 전환 및 일정 시간 뒤 -> Leaving: 매장 떠남
    /// </summary>
    
    public Customer Customer { get; }
    public VisitState VisitState { get; }
    public BakeWaitingState BakeWaitingState { get; }
    public OrderWaitingState OrderWaitingState { get; }
    public BuyState BuyState { get; }
    public CheckOutState CheckOutState { get; } // 식사 또는 포장
    public LeavingState LeavingState { get; }
    
    public CustomerStateMachine(Customer customer)
    {
        
        Customer = customer;

        VisitState = new VisitState(this);
        BakeWaitingState = new BakeWaitingState(this);
        OrderWaitingState = new OrderWaitingState(this);
        BuyState = new BuyState(this);
        CheckOutState = new CheckOutState(this);
        LeavingState = new LeavingState(this);
    }
}