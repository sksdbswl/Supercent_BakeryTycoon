using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour, IProductTarget
{
    public CustomerStateMachine CustomerStateMachine { get; private set; }
    public Animator animator;
    public NavMeshAgent navAgent { get; private set; }
    public PooledObject PooledObject { get; set; }
    public CustomerData customerData; 
    [field:SerializeField] public Transform BreadTransform { get; set; }
    public Stack<Product> PickedUpBreads { get; set; } = new Stack<Product>();
    public NavPoint currentPoint;
    public GameObject currentPaperBox { get; set; }
    public CustomerUI CustomerUI { get; set; }
    public Transform startPos;
    
    private void OnEnable()
    {
        PickedUpBreads.Clear();       
        currentPoint = null;
        currentPaperBox = null;
        navAgent.ResetPath();        
        CustomerStateMachine = new CustomerStateMachine(this);
        InitCustomer();              
    }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        PooledObject =GetComponent<PooledObject>();
        CustomerUI = GetComponent<CustomerUI>();
        CustomerStateMachine = new CustomerStateMachine(this);
        
    }

    private void Start()
    {
        InitCustomer();
    }

    public void InitCustomer()
    {
        var template = GameManager.Instance.CustomerTable.GetRandomCustomer();
        customerData = ScriptableObject.CreateInstance<CustomerData>();
    
        customerData.customerName = template.customerName;
        customerData.desiredBread = template.desiredBread;
        customerData.desiredBreadId = template.desiredBreadId;
        customerData.quantity = template.quantity;
        customerData.pickedUpCount = 0;
        customerData.wantsToEatIn = template.wantsToEatIn;
        animator.SetTrigger(CustomerAnimationController.Move);

        StartCoroutine(CheckStartPositionCoroutine());
    }
    
    private IEnumerator CheckStartPositionCoroutine()
    {
        if (startPos == null)
            yield break;

        while (Vector3.Distance(transform.position, startPos.position) > 0.1f)
        {
            navAgent.SetDestination(startPos.position);
            yield return null; 
        }

        CustomerStateMachine.ChangeState(CustomerStateMachine.VisitState);
    }
    
    private void Update()
    {
        CustomerStateMachine.Update();
    }

    public void OnPointAssigned(NavPoint point)
    {
        navAgent.SetDestination(point.transform.position);
    }
    
    public bool ArriveCheck()
    {
        if (!navAgent.pathPending &&
            navAgent.remainingDistance <= 0.1f)
        {
            animator.SetTrigger(CustomerAnimationController.Idle);
            return true;
        }

        return false;
    }

    public void MoveToEat(Transform target)
    {
        // 대기 루틴 시작
        navAgent.SetDestination(target.transform.position);
    }
    
    public void Payment(GameManager.PaymentType type)
    {
        int BreadCount = PickedUpBreads.Count;
        int totalCost = 0;
        
        for (int i = 0; i < BreadCount; i++)
        {
            totalCost += customerData.desiredBread.price;
        }
        
        GameManager.Instance.SpawnMoney(type, totalCost);
    }
}