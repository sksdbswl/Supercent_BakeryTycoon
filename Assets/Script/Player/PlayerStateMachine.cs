
public class PlayerStateMachine : StateMachine
{
    /// <summary>
    /// 상태(State):
    /// Idle: 대기 
    /// Baking: 빵 픽업
    /// Restocking: 진열대에 빵 채우기
    /// Payment: 계산 처리

    ///전환(Transition) 예시:
    /// Idle → Pickup: 오븐 위치 
    /// Idle → Restocking: 진열대 위치
    /// Idle → Upgrading: 메뉴 선택
    /// </summary>
    
    public Player Player { get; }
    public IdleState IdleState { get; }
    public BakingState BakingState { get; }
    public RestockState RestockState { get; }
    public PaymentState PaymentState { get; }
    
    public PlayerStateMachine(Player player)
    {
        Player = player;

        IdleState = new IdleState(this);
        BakingState = new BakingState(this);
        RestockState = new RestockState(this);
        PaymentState = new PaymentState(this);
    }
}