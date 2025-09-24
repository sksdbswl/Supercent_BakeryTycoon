using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    public Animator animator { get; private set; }
    public PlayerMover Mover { get; private set; } 
    public int Money { get; set; }
    
    private void Awake()
    {
        // 초기 플레이어 설정
        animator = GetComponentInChildren<Animator>();
        Mover = GetComponent<PlayerMover>(); 
        
        // 초기 플레이어 생성 및 FSM 시작 선언
        PlayerStateMachine = new PlayerStateMachine(this);
        PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);

        // 초기 플레이어 자산 설정
        Money = 10000;
    }

    private void Update()
    {
        PlayerStateMachine.Update();
    }
    
    public event System.Action<int> OnMoneyChanged;

    public void AddMoney(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
    }

    public void SpendMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            OnMoneyChanged?.Invoke(Money);
        }
    }
}