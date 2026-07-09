public class EnemyTraceState : EnemyState
{
    public EnemyTraceState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        // 플레이어가 추적 해제 범위 밖으로 나가면 원래 위치로 복귀한다.
        if (enemy.IsPlayerLost())
        {
            stateMachine.ChangeState(enemy.ReturnState);
            return;
        }

        // 플레이어가 공격 범위 안에 들어오면 공격 상태로 전환한다.
        if (enemy.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(enemy.AttackState);
            return;
        }

        // 플레이어가 공격 범위 밖에 있고 추적 범위 안에 있으면 계속 따라간다.
        enemy.MoveTo(enemy.Player.position);
    }

    public override void Exit()
    {
    }
}
