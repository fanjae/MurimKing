public abstract class EnemyState
{
    // 상태에서 공통으로 사용할 Enemy, StateMachine
    protected readonly Enemy enemy;
    protected readonly EnemyStateMachine stateMachine;

    protected EnemyState(Enemy enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
