using UnityEngine;

public class PlayerDieState : PlayerStateBase
{
    protected override void OnEnable()
    {
        base.OnEnable();

        animationController.PlayDie();
    }
}