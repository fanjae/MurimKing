using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IPlayerInput
{
    // 이동 값
    public Vector2 Movement { get; private set; }
    public Vector2 LookDelta { get; private set; }

    // 점프 여부
    public bool JumpPressed { get; private set; }
    public bool CombatPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool CameraRotatePressed { get; private set; }
    public bool InventoryPressed { get; private set; }


    private InputAction moveAction;

    private InputAction jumpAction;

    // 전투 관련 입력
    private InputAction combatAction;
    private InputAction attackAction;

    // 카메라 관련 입력
    private InputAction cameraAction;

    // 인벤토리 입력
    private InputAction inventoryAction;


    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        combatAction = InputSystem.actions.FindAction("Combat");
        attackAction = InputSystem.actions.FindAction("Attack");
        cameraAction = InputSystem.actions.FindAction("Camera");

        inventoryAction = InputSystem.actions.FindAction("Inventory");
    }
    private void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();
        LookDelta = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;

        JumpPressed = jumpAction.WasPressedThisFrame();
        CombatPressed = combatAction.WasPressedThisFrame();
        AttackPressed = attackAction.WasPressedThisFrame();
        CameraRotatePressed = cameraAction.IsPressed();

        InventoryPressed = inventoryAction.WasPressedThisFrame();

    }
}
