using UnityEngine;

public class BakingState : PlayerBaseState
{
    public BakingState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    private BreadSpawner currentSpawner;
    
    public override void Enter()
    {
        Debug.Log("BakingState Enter");   
        
        //TryPickUpBread();
    }

    public override void Update()
    {
        Debug.Log($"BakingState:: {stateMachine.Player.Bread.Count}");   
        
        // 최대 소지량 도달 시 상태 전환
        // if (stateMachine.Player.Bread >= 10)
        // {
        //     stateMachine.ChangeState(stateMachine.IdleState);
        //     return;
        // }

        // 플레이어 이동 입력 체크
        // Vector3 move = stateMachine.Player.Mover.GetMoveDirection();
        // if (move.sqrMagnitude > 0.01f)
        // {
        //     stateMachine.ChangeState(stateMachine.MoveState);
        //     return;
        // }

        // 일정 시간마다 빵 자동 픽업 가능
        TryPickUpBread();
    }
    
    private void TryPickUpBread()
    {
        // if (ovenUnit == null) return;
        //
        // // 오븐에 남은 빵 중 하나 가져오기
        // GameObject breadGO = ovenUnit.GetAvailableBread();
        // if (breadGO == null) return;
        //
        // // 플레이어 손 위치로 이동
        // breadGO.transform.SetParent(player.BreadHolder);
        // breadGO.transform.localPosition = Vector3.zero;
        // breadGO.transform.localRotation = Quaternion.identity;
        //
        // // 물리 비활성화
        // Rigidbody rb = breadGO.GetComponent<Rigidbody>();
        // if (rb != null) rb.isKinematic = true;
        //
        // Collider col = breadGO.GetComponent<Collider>();
        // if (col != null) col.enabled = false;
        //
        // player.Bread++;
        // player.animator.SetBool("HasBread", true);
    }
}