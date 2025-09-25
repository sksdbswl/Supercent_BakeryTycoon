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
        
        // 중복 실행 방지
        if (isClosedContainer) StartCoroutine(GetProductsCoroutine());
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

    private IEnumerator GetProductsCoroutine()
    {
        var term = new WaitForSeconds(0.5f);
        while (isClosedContainer)
        {
            if (Container is Oven)
            {
                var product = Container.GetProduct();
                
                if (product == null)
                {
                    yield return term;
                    continue;
                }
                
                product.MoveTo(this, Product.GoalType.Player);
            }
            else if (Container is Showcase)
            {
                var showcase = Container as Showcase;

                Debug.Log(showcase);
                
                if (PickedUpBreads.Count == 0)
                {
                    Debug.Log("쇼케이스 사용중지");
                    // 빵이 없으면 쇼케이스 사용중 아님
                    showcase.SetBusy(false);
                    
                    yield return term;
                    continue;
                }

                var bread = PickedUpBreads.Pop();
                // 빵이 있으면 쇼케이스 사용중 => 손님 사용불가
                showcase.Exhibition(bread);
                bread.MoveTo(this, Product.GoalType.Showcase);
            }

            yield return term; 
        }
    }
}