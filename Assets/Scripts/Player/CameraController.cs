using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float distance = 6.0f;
    [SerializeField] private float height = 3.0f;
    [SerializeField] private float rotateSpeed = 120.0f;

    private float yaw = 0.0f;
    private IPlayerInput input;

    private void Awake()
    {
        if (target != null)
        {
            input = target.GetComponent<IPlayerInput>();
        }
    }

    private void LateUpdate()
    {
        if (target == null || input == null) return;

        if (input.CameraRotatePressed)
        {
            yaw += input.LookDelta.x * rotateSpeed * Time.deltaTime;
        }

        Quaternion rotation = Quaternion.Euler(0.0f, yaw, 0.0f);
        Vector3 offset = rotation * new Vector3(0.0f, height, -distance);

        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}