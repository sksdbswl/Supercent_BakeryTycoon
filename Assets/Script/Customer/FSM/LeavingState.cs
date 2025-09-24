using UnityEngine;

public class LeavingState : CustomerBaseState
{
    public LeavingState(CustomerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Transform exitPoint = GameManager.Instance.customerSpawner.spawnPosition;
        stateMachine.Customer.navAgent.SetDestination(exitPoint.position);
        stateMachine.Customer.animator.SetTrigger(CustomerAnimationController.Move);
    }

    public override void Update()
    {
        float distance = Vector3.Distance(
            stateMachine.Customer.transform.position,
            GameManager.Instance.customerSpawner.spawnPosition.position
        );

        if (distance < 0.2f) // 도착 판정 임계값
        {
            stateMachine.Customer.navAgent.isStopped = true;

            // 풀에 반환
            GenericPoolManager.Instance.Release(
                stateMachine.Customer.PooledObject.OriginPrefab,
                stateMachine.Customer.gameObject
            );
        }
    }
}