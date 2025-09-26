using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour, IProductTarget
{
    public CustomerStateMachine CustomerStateMachine { get; private set; }
    public Animator animator;
    public NavMeshAgent navAgent { get; private set; }
    public PooledObject PooledObject { get; set; }
    // SO 데이터
    public CustomerData customerData; 
    [field:SerializeField] public Transform BreadTransform { get; set; }
    public Stack<Product> PickedUpBreads { get; set; } = new Stack<Product>();
    public NavPoint currentPoint;
    public GameObject currentPaperBox { get; set; }
    
    private void OnEnable()
    {
        InitCustomer();              
        PickedUpBreads.Clear();       
        currentPoint = null;
        currentPaperBox = null;
        navAgent.ResetPath();        
        CustomerStateMachine = new CustomerStateMachine(this);
    }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        PooledObject =GetComponent<PooledObject>();
        CustomerStateMachine = new CustomerStateMachine(this);
        
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
            return true;
        }

        return false;
    }

    public void MoveToEat(Transform target)
    {
        // 대기 루틴 시작
        navAgent.SetDestination(target.transform.position);
    }
}