using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerStateManager stateManager;
    [SerializeField] private CharacterController controller;

    // 점프 및 낙하 속도
    private Vector3 verticalVelocity;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();

        if (controller == null) controller = GetComponent<CharacterController>();

        stateManager = GetComponent<PlayerStateManager>();
    }

    private void OnAnimatorMove()
    {
        if (controller == null || !controller.enabled)
            return;
        
        // 이전 이동 결과 기준 지면 확인
        IsGrounded = controller.isGrounded;

        // 지면에 있을 때 아래 방향 속도 고정(누적 안되도록)
        if (IsGrounded && verticalVelocity.y < 0f) verticalVelocity.y = -1f;

        verticalVelocity += Physics.gravity * Time.deltaTime;

        // 애니메이션 Root Mtion 이동량 및 점프, 낙하 수직 이동량을 추가하여 이동량 적용
        Vector3 motion = animator.deltaPosition;
        motion += verticalVelocity * Time.deltaTime;

        controller.Move(motion);

        // 이동 결과 기준 지면 상태 갱신
        IsGrounded = controller.isGrounded;
    }

    public void JumpVelocitySet(float jumpSpeed)
    {
        verticalVelocity.y = jumpSpeed;
    }
}