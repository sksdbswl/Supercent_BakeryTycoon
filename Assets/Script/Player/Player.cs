using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour, IProductTarget
{
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    public Animator animator { get; private set; }
    public PlayerMover Mover { get; private set; } 
    
    public event Action<int> OnMoneyChanged;
    public int Money { get; set; }
    
    private bool isClosedContainer = false;
    public IProductContainer Container;
    
    public CharacterController characterController;
    
    [field:SerializeField] public Transform BreadTransform { get; set; }
    public Stack<Product> PickedUpBreads { get; set; } = new Stack<Product>();
    
    private void Awake()
    {
        // 초기 플레이어 설정
        animator = GetComponentInChildren<Animator>();
        Mover = GetComponent<PlayerMover>();
        characterController = GetComponent<CharacterController>();
        
        // 초기 플레이어 생성 및 FSM 시작 선언
        PlayerStateMachine = new PlayerStateMachine(this);
        PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);

        // 초기 플레이어 자산 설정
        Money = 10000;
    }

    private void Update()
    {
        PlayerStateMachine.Update();
        
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
    
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
    
    private void OnTriggerEnter(Collider other)
    {
        var container = other.GetComponent<IProductContainer>();
        if (container == null) return;

        Container = container;
        isClosedContainer = true;

        if (isClosedContainer) StartCoroutine(GetProductsCoroutine());
    }

    private IEnumerator GetProductsCoroutine()
    {
        var term = new WaitForSeconds(0.5f);

        while (isClosedContainer)
        {
            switch (Container)
            {
                case Oven oven:
                    var product = oven.GetProduct();
                    if (product != null)
                        product.MoveTo(this, Product.GoalType.Player);
                    break;

                case Showcase showcase:
                    if (PickedUpBreads.Count == 0)
                    {
                        Debug.Log("쇼케이스 사용중지");
                        showcase.SetBusy(false);
                        break;
                    }

                    var bread = PickedUpBreads.Pop();
                    showcase.Exhibition(bread);
                    bread.MoveTo(this, Product.GoalType.Showcase);
                    break;
                
                case Cashier cashier:
                    //1. 앞에 손님이 있는지 ? 
                
                default:
                    Debug.LogWarning($"알 수 없는 IProductContainer 타입: {Container.GetType()}");
                    break;
            }

            yield return term;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var container = other.GetComponent<IProductContainer>();
        if (container == null) return;

        if (container == Container)
        {
            if (container is Showcase showcase)
            {
                showcase.SetBusy(false);
            }

            Container = null;
            isClosedContainer = false; 
        }
    }
}