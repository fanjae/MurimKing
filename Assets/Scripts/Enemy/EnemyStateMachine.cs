using UnityEngine;

// 몬스터의 현재 상태를 관리하는 상태 머신 클래스
// 상태 전환과 현재 상태의 Update 실행을 담당한다.
public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }

    // 새로운 상태로 전환하는 메서드
    public void ChangeState(EnemyState newState)
    {
        // 잘못된 상태가 들어오면 전환하지 않고 경고를 출력한다.
        if (newState == null)
        {
            Debug.LogWarning("EnemyStateMachine.ChangeState() failed: newState is null.");
            return;
        }

        if (CurrentState == newState)
            return;

        // 기존 상태가 있다면 종료하고, 새로운 상태로 변경한 뒤 진입한다.
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    // 현재 상태의 Update를 실행.
    public void Update()
    {
        CurrentState?.Update();
    }
}
