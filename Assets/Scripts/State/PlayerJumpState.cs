
using UnityEngine;

public class PlayerJumpState : PlayerStateBase
{
    [SerializeField] private float jumpSpeed = 7.0f;

    protected override void OnEnable()
    {
        base.OnEnable();

        animationController.PlayJump();
        movementController.JumpVelocitySet(jumpSpeed);
    }

    private void Update()
    {
        if (movementController.IsGrounded)
        {
            if (InputManager.MoveMent == Vector2.zero) manager.SetState(PlayerStateManager.State.Idle);
            else manager.SetState(PlayerStateManager.State.Move);
        }
    }
}