using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour, IProductTarget
{
    public PlayerUI playerUI;
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    public Animator animator { get; private set; }
    public PlayerMover Mover { get; private set; }
    public event Action<int> OnMoneyChanged;
    public int Money { get; set; }

    private bool isClosedContainer = false;
    public IProductContainer Container;

    [field: SerializeField] public Transform BreadTransform { get; set; }
    public Stack<Product> PickedUpBreads { get; set; } = new Stack<Product>();
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        Mover = GetComponent<PlayerMover>();
        playerUI = GetComponent<PlayerUI>();
            
        PlayerStateMachine = new PlayerStateMachine(this);
        PlayerStateMachine.ChangeState(PlayerStateMachine.IdleState);

        Money = 0;
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

        GameObject triggerObj = GetTriggerObject(container);
        if (triggerObj != null)
        {
            TriggerZoneScaleUp(triggerObj);
        }
        
        if (isClosedContainer) StartCoroutine(GetProductsCoroutine());
    }

    public Customer Customer { get; set; }

    private IEnumerator GetProductsCoroutine()
    {
        var term = new WaitForSeconds(0.2f);

        while (isClosedContainer)
        {
            switch (Container)
            {
                case Oven oven:
                    if (PickedUpBreads.Count >= 10)
                    {
                        playerUI.MaxIcon.SetActive(true);
                        break;
                    }

                    if (oven.BakedCheck())
                    {
                        var queueBread = oven.breadQueue.Dequeue();
                        
                        queueBread.MoveTo(this, Product.GoalType.Player);
                        oven.GetProduct();
                        
                        GameManager.Instance.OnStepComplete(GameManager.TutorialStep.PickupBread);
                    }
                       
                    break;

                case Showcase showcase:
                    if (PickedUpBreads.Count == 0)
                    {
                        showcase.SetBusy(false);
                        break;
                    }

                    var bread = PickedUpBreads.Pop();
                    playerUI.MaxIcon.SetActive(false);
                    showcase.Exhibition(bread);
                    bread.MoveTo(this, Product.GoalType.Showcase);
                    GameManager.Instance.OnStepComplete(GameManager.TutorialStep.ShowcaseArrow);
                    
                    break;

                case Cashier cashier:
                    if (Customer == null)
                    { 
                        var currentCustomer = cashier.GetCurrentCustomer();
                        if (currentCustomer != null)
                        {
                            Customer = currentCustomer;
                            GameObject paperBox = cashier.SpawnPaperBox();
                            Customer.currentPaperBox = paperBox;
                            Customer.StartCoroutine(Customer.CustomerStateMachine.BuyState
                                .FinishAfterDelay(this, paperBox));
                            
                            GameManager.Instance.OnStepComplete(GameManager.TutorialStep.CashierArrow);
                        }
                    }

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

            GameObject triggerObj = GetTriggerObject(container);
            if (triggerObj != null)
            {
                TriggerZoneScaleDown(triggerObj);
            }
            
            Container = null;
            isClosedContainer = false;
        }
    }

    /// <summary>
    /// 플레이어 유닛 트리거 확인
    /// </summary>
    private GameObject GetTriggerObject(IProductContainer container)
    {
        switch (container)
        {
            case Oven oven: return oven.TriggerCheck;
            case Showcase showcase: return showcase.TriggerCheck;
            case Cashier cashier: return cashier.TriggerCheck;
            default: return null;
        }
    }
    
    public void TriggerZoneScaleUp(GameObject obj, float scaleAmount = 0.2f, float duration = 0.3f)
    {
        obj.transform.DOKill();
        obj.transform.DOScale(obj.transform.localScale + Vector3.one * scaleAmount, duration)
            .SetEase(Ease.OutBack);
    }

    public void TriggerZoneScaleDown(GameObject obj, float duration = 0.3f)
    {
        obj.transform.DOKill();
        obj.transform.DOScale(Vector3.one, duration) 
            .SetEase(Ease.OutBack);
    }
}