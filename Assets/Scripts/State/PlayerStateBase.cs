using UnityEngine;

public class PlayerStateBase : MonoBehaviour
{
    protected Transform refTransform;
    protected PlayerStateManager manager;
    protected PlayerMovementController movementController;
    protected AnimationController animationController;

    protected virtual void OnEnable()
    {
        refTransform = transform;
        manager = GetComponent<PlayerStateManager>();
        movementController = GetComponent<PlayerMovementController>();
        animationController = GetComponentInChildren<AnimationController>();
    }
}