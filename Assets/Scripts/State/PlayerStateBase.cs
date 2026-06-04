using UnityEngine;

public class PlayerStateBase : MonoBehaviour
{
    protected Transform refTransform;
    protected CharacterController characterController;
    protected PlayerStateManager manager;

    protected virtual void OnEnable()
    {
        refTransform = transform;
        characterController = GetComponent<CharacterController>();
        manager = GetComponent<PlayerStateManager>();
    }

    protected virtual void Update()
    {
        // 중력 적용
        characterController.Move(Physics.gravity * Time.deltaTime);
    }
}