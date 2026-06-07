using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float distance = 6.0f;
    [SerializeField] private float height = 3.0f;
    [SerializeField] private float rotateSpeed = 120.0f;

    private float yaw = 0.0f;

    private void LateUpdate()
    {
        if (target == null) return;

        // 우클릭 중 마우스의 이동량으로 카메라 회전 처리
        if (InputManager.IsRightMousePressed) 
        {
            float mouseX = Mouse.current.delta.ReadValue().x;
            yaw += mouseX * rotateSpeed * Time.deltaTime;
        }

         
        Quaternion rotation = Quaternion.Euler(0.0f, yaw, 0.0f);

        // 카메라 위치는 캐릭터의 뒤쪽에서 위쪽을 보도록 처리
        Vector3 offset = rotation * new Vector3(0.0f, height, -distance);

        // 카메라 위치는 캐릭터의 상체를 바라보는 것을 전제하되, offset을 기준으로 캐릭터 위치에 더해서 카메라 위치를 결정
        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}