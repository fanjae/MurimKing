using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    // 공격 상태 진입시 즉시 공격 가능하도록 타이머를 0으로 변경
    public override void Enter()
    {

    }

    public override void Update()
    {
        // 플레이어가 추적 해제 범위 밖으로 나가면 원래 위치로 복귀
        if (enemy.IsPlayerLost())
        {
            stateMachine.ChangeState(enemy.ReturnState);
            return;
        }
        
        // 플레이어가 공격 범위 안에 들어오면 공격 상태로 전환
        if (!enemy.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(enemy.TraceState);
            return;
        }

        // 공격 중에 플레이어 방향 쪽 바라보도록 회전
        Vector3 direction = enemy.Player.position - enemy.transform.position;
        enemy.RotateTo(direction);

        // 공격
        enemy.Attack();
    }

    public override void Exit()
    {
    }
}
