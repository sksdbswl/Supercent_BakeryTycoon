public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public IdleState IdleState { get; }
    public MoveState MoveState { get; }
    
    public PlayerStateMachine(Player player)
    {
        Player = player;

        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
    }
}