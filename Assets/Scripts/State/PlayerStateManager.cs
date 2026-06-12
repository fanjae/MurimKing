using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum State
    {
        None = -1, Idle, Move, Jump, Die, Length
    }

    // 플레이어의 현재 상태 표시 및 상태 배열
    [SerializeField] private State state = State.None;
    [SerializeField] private PlayerStateBase[] states;

    [SerializeField] private bool isCombat = false;
    [SerializeField] private float combatReturnTime = 5.0f;
    [SerializeField] private float attackStaminaCost = 4.0f;
    [SerializeField] private float attackCoolTime = 0.2f;

    private PlayerMovementController movementController;
    private AnimationController animationController;
    private PlayerStamina stamina;
    private IPlayerInput input;


    private float combatTimer = 0.0f;
    public bool IsNextAttackReserved { get; private set; } = false;
    private float attackTimer = 0.0f;

    public State CurrentState => state;



    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        animationController = GetComponentInChildren<AnimationController>();
        stamina = GetComponent<PlayerStamina>();
        input = GetComponent<IPlayerInput>();
    }

    private void OnEnable()
    {
        SetState(State.Idle);
    }

    private void Update()
    {
        if (state == State.Die) return; // 사망 상태에서는 모든 입력처리가 다 막힘
        if (input == null) return;

        if (attackTimer > 0.0f)
        {
            attackTimer -= Time.deltaTime;
        }

        /// 점프일때는 대기
        if (state == State.Jump) return;

        HandleCombat();

        // 지면에 있고, 점프 입력이 들어온 경우 처리
        if (movementController.IsGrounded && input.JumpPressed && CanJumpCurrentState())
        {
            SetState(State.Jump);
            return;
        }

        if (input.Movement == Vector2.zero)
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

    private void HandleCombat()
    {
        // 전투 모드 On/Off
        if (input.CombatPressed)
        {
            isCombat = !isCombat;
            combatTimer = 0.0f;
            animationController.SetCombat(isCombat);
        }

        if (!isCombat) return;

        combatTimer += Time.deltaTime;

        if (input.AttackPressed && attackTimer <= 0.0f)
        {
            attackTimer = attackCoolTime;
            combatTimer = 0.0f;

            if (stamina != null && !stamina.CanUse(attackStaminaCost)) // 스태미나 체크
            {
                return;
            }

            // 애니메이션 재생 여부 확인
            if (IsAttacking())
            {
                if (CanReserveNextAttack())
                {
                    IsNextAttackReserved = true;
                }
                else
                {
                    ClearNextAttackReserved();
                }

                return;
            }

            ClearNextAttackReserved();
            animationController.SetAttackTrigger();
        }

        if (combatTimer >= combatReturnTime)
        {
            isCombat = false;
            animationController.SetCombat(false);
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
    private bool IsAttacking()
    {
        AnimatorStateInfo info = animationController.GetCurrentStateInfo();

        return info.IsName("Attack_01") ||
               info.IsName("Attack_02") ||
               info.IsName("Attack_03");
    }
    public bool IsAttackingNow()
    {
        return IsAttacking();
    }

    public void ClearNextAttackReserved()
    {
        IsNextAttackReserved = false;
    }

    public bool UseAttackStamina()
    {
        if (stamina == null) return true;

        return stamina.TryUse(attackStaminaCost);
    }

    private bool CanReserveNextAttack()
    {
        AnimatorStateInfo info = animationController.GetCurrentStateInfo();

        return info.IsName("Attack_01") ||
               info.IsName("Attack_02");
    }
}