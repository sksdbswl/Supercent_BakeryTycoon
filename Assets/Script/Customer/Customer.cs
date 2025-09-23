using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public CustomerStateMachine CustomerStateMachine { get; private set; }
    public Animator animator;
    public NavMeshAgent navAgent { get; private set; }
    
    // SO 데이터
    public CustomerData customerData; 
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        
        CustomerStateMachine = new CustomerStateMachine(this);
        //CustomerStateMachine.ChangeState(CustomerStateMachine.VisitState);
        
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