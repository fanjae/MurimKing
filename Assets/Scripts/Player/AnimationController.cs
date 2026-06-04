using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator refAnimator;
    private void OnEnable()
    {
        if (refAnimator == null)
        {
            refAnimator = GetComponentInParent<Animator>();
        }
    }
    

    // 상태 교체를 애니메이터에 알림.
    public void OnStateChanged(PlayerStateManager.State newState)
    {
        if (refAnimator == null) return;

        refAnimator.SetInteger("State", (int)newState);
    }
}
