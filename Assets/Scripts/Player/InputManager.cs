using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // 이동 값
    public static Vector2 MoveMent;

    // 점프 여부
    public static bool IsJump { get; private set; } = false;


    private InputAction moveAction;

    private InputAction jumpAction;


    private void Awake()
    {
        if(moveAction == null) moveAction = InputSystem.actions.FindAction("Move");
        if(jumpAction == null) jumpAction = InputSystem.actions.FindAction("Jump");
    }
    private void Update()
    {
        MoveMent = moveAction.ReadValue<Vector2>();
        IsJump = jumpAction.WasPressedThisFrame();
    }
}
