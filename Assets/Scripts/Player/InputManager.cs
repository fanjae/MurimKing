using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // 이동 값
    public static Vector2 MoveMent;

    // 점프 여부
    public static bool IsJump { get; private set; } = false;

    public static bool IsCombat { get; private set; } = false;

    public static bool IsRightMousePressed { get; private set; } = false;


    private InputAction moveAction;

    private InputAction jumpAction;

    // 전투 관련 입력
    private InputAction combatAction;

    // 카메라 관련 입력
    private InputAction cameraAction;


    private void Awake()
    {
        if(moveAction == null) moveAction = InputSystem.actions.FindAction("Move");
        if(jumpAction == null) jumpAction = InputSystem.actions.FindAction("Jump");
        if (combatAction == null) combatAction = InputSystem.actions.FindAction("Combat");

        if (cameraAction == null) cameraAction = InputSystem.actions.FindAction("Camera");
    }
    private void Update()
    {
        MoveMent = moveAction.ReadValue<Vector2>();
        IsJump = jumpAction.WasPressedThisFrame();

        IsCombat = combatAction.WasPressedThisFrame();
        IsRightMousePressed = cameraAction.IsPressed(); // 누르고 있는 동안 true
    }
}
