using UnityEngine;

public interface IPlayerInput
{
    Vector2 Movement { get; }
    Vector2 LookDelta { get; }

    bool JumpPressed { get; }
    bool CombatPressed { get; }
    bool AttackPressed { get; }
    bool CameraRotatePressed { get; }

    bool InventoryPressed { get; }

    bool InteractPressed { get; }
}