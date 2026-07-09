public class EnemyIdleState : EnemyState
{
    // 대기 상태를 담당하는 클래스
    // 플레이어가 탐지 범위 안에 들어오기 전까지 아무 행동 없이 대기한다.
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }


    public override void Enter()
    {
        
    }

    public override void Update()
    {
        // 플레이어가 탐지 범위 안에 들어오면 추적 상태로 전환.
        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.TraceState);
        }
    }

    public override void Exit()
    {
       
    }
}
