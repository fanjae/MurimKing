using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // 이동 값
    public static Vector2 MoveMent;

    private InputAction moveAction;

    private void Awake()
    {
        if (moveAction == null) moveAction = InputSystem.actions.FindAction("Move");
    }
    private void Update()
    {
        MoveMent = moveAction.ReadValue<Vector2>();
    }
}
