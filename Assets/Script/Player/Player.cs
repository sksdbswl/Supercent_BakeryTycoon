using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    public Animator animator { get; private set; }
    public PlayerMover Mover { get; private set; } 
    
    public event Action<int> OnMoneyChanged;
    public int Money { get; set; }
    
    public Transform BreadTransform;
    public Stack<Product> PickedUpBreads { get; private set; } = new Stack<Product>();
        
    private bool isClosedContainer = false;
    public IProductContainer Container;
    
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
                
                product.MoveTo(this, BreadTransform, Product.GoalType.Player);
            }
            else if (Container is Showcase)
            {
                if (PickedUpBreads.Count == 0)
                {
                    yield return term;
                    continue;
                }

                var bread = PickedUpBreads.Pop();
                var showcase = Container as Showcase;

                showcase.Exhibition(bread);
                bread.MoveTo(this, showcase.transform, Product.GoalType.Showcase);
            }

            yield return term; 
        }
    }
}