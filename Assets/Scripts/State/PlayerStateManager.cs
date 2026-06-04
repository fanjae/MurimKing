using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateManager : MonoBehaviour
{
    // 플레이어의 State를 나타내는 열거형
    public enum State
    {
        None = -1, Idle, Move, Length
    }

    [SerializeField] private State state = State.None;
    [SerializeField] private PlayerStateBase[] states;

    [SerializeField] private UnityEvent<State> OnStateChanged;

    private void OnEnable()
    {
        SetState(State.Idle);
    }
    private void Update()
    {
        if(InputManager.MoveMent == Vector2.zero)
        {
            // 이동 없음.
            SetState(State.Idle);
        }
        else
        {
            SetState(State.Move);
        }
    }
    public void SetState(State newState)
    {
        if (state == newState) return;

        if (state != State.None) // 기존 상태 비활성화
        {
            states[(int)newState].enabled = false;
        }

        // 상태 갱신
        states[(int)newState].enabled = true;
        state = newState;

        // 상태 변경을 알림.
        OnStateChanged?.Invoke(state);
    }
}
