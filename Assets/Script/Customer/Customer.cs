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
        customerData = GameManager.Instance.CustomerTable.GetRandomCustomer();
    }

    private void Update()
    {
        CustomerStateMachine.Update();
    }


}