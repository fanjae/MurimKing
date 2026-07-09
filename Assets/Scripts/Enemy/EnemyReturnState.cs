using UnityEngine;

public class EnemyReturnState : EnemyState
{
    public EnemyReturnState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        // 복귀 애니메이션이 있으면 여기에서 실행한다.
    }

    public override void Update()
    {
        // 복귀 중 플레이어가 다시 탐지되면 추적 상태로 전환한다.
        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.TraceState);
            return;
        }

        // 현재 위치와 시작 위치 사이의 거리를 계산한다.
        float distanceToOrigin = Vector3.Distance(enemy.transform.position, enemy.OriginPosition);

        // 시작 위치에 충분히 가까워졌다면 대기 상태로 전환한다.
        if (distanceToOrigin <= enemy.ReturnStopDistance)
        {
            stateMachine.ChangeState(enemy.IdleState);
            return;
        }

        // 아직 시작 위치에 도착하지 않았다면 시작 위치로 이동한다.
        enemy.MoveTo(enemy.OriginPosition);
    }

    public override void Exit()
    {
    }
}
