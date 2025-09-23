
public class CustomerStateMachine : StateMachine
{
    /// <summary>
    /// 상태(State) 예시
    /// VisitState : 방문 상태 ( 걸어오는 모션 )
    /// BakeWaiting: 빵 대기열에서 기다리는 상태
    /// OrderWaiting: 계산 대기
    /// BuyState: 계산
    /// Cheakout - Eating/ToGo: 식사 또는 포장
    /// Leaving: 매장 떠남

    /// 전환(Transition) 예시:
    /// Waiting → Ordering: 대기 순서가 되면
    /// Ordering → Paying: 주문 완료 시
    /// Paying → Eating/ToGo: 결제 완료 시
    /// Eating/ToGo → Leaving: 식사/포장 완료 시
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