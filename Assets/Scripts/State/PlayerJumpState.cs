
using UnityEngine;

public class PlayerJumpState : PlayerStateBase
{
    [SerializeField] private float jumpSpeed = 7.0f;

    protected override void OnEnable()
    {
        base.OnEnable();

        animationController.PlayJump(); // 점프 애니메이션 재생
        movementController.JumpVelocitySet(jumpSpeed); // verticalVelocity와 중력 계산으로 처리
    }

    private void Update()
    {
        if (movementController.IsGrounded)
        {
            if (input.Movement == Vector2.zero) manager.SetState(PlayerStateManager.State.Idle);
            else manager.SetState(PlayerStateManager.State.Move);
        }
    }
}