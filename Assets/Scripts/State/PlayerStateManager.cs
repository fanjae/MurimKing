using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum State
    {
        None = -1, Idle, Move, Jump, Length
    }

    [SerializeField] private State state = State.None;
    [SerializeField] private PlayerStateBase[] states;

    private PlayerMovementController movementController;
    private AnimationController animationController;

    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        animationController = GetComponentInChildren<AnimationController>();
    }

    private void OnEnable()
    {
        SetState(State.Idle);
    }

    private void Update()
    {
        /// 점프일때는 대기
        if (state == State.Jump) return;

        // 지면에 있고, 점프 입력이 들어온 경우 처리
        if (movementController.IsGrounded && InputManager.IsJump && CanJumpCurrentState())
        {
            SetState(State.Jump);
            return;
        }

        if (InputManager.MoveMent == Vector2.zero)
        {
            // 이동 없음.
            SetState(State.Idle);
        }
        else
        {
            // 이동
            SetState(State.Move);
        }

    }

    private bool CanJumpCurrentState()
    {
        return state == State.Idle || state == State.Move;
    }

    public void SetState(State newState)
    {
        if (state == newState) return;
        if (newState <= State.None || newState >= State.Length) return;

        if (state != State.None) // 기존 상태 비활성화
        {
            states[(int)state].enabled = false;
        }

        // 상태 갱신.
        states[(int)newState].enabled = true;
        state = newState;

        if (animationController != null)
        {
            // 상태 변경.
            animationController.OnStateChanged(state);
        }
        else
        {
            Debug.LogWarning("AnimationController is null");
        }
    }
}