using UnityEngine;

public class PlayerMoveState : PlayerStateBase
{
    [SerializeField] private float rotationSpeed = 540.0f;

    protected override void Update()
    {
        base.Update();

        Vector3 direction = new Vector3(InputManager.MoveMent.x, 0.0f, InputManager.MoveMent.y);

        if(direction.sqrMagnitude > 1.0f) // 벡터 길이의 제곱이 1을 넘으면 보정
        {
            direction.Normalize();
        }

        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            refTransform.rotation = Quaternion.RotateTowards(refTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
