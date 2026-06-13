using UnityEngine;

public class PlayerMoveState : PlayerStateBase
{
    [SerializeField] private float rotationSpeed = 540.0f;

    private void Update()
    {
        // 입력된 이동 방향 가져옴
        Vector2 movement = input.Movement;

        Transform cam = Camera.main.transform;

        // 카메라의 앞 방향을 가져오되, y축은 제거해서 수평 방향만 사용
        Vector3 camForward = cam.forward; 
        camForward.y = 0.0f;
        camForward.Normalize();

        // 카메라의 오른쪽 방향도 마찬가지로 수평 방향만 사용
        Vector3 camRight = cam.right;
        camRight.y = 0.0f;
        camRight.Normalize();

        // 입력값을 카메라 기준 방향으로 변환
        Vector3 direction = camForward * movement.y + camRight * movement.x;

        if (direction.sqrMagnitude > 1.0f) direction.Normalize();  // 벡터 길이의 제곱이 1을 넘으면 보정

        // 이동 입력이 있을때 회전 처리
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            refTransform.rotation = Quaternion.RotateTowards(
                refTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
