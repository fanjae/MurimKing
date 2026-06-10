using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator refAnimator;
    private void Awake()
    {
        refAnimator = GetComponentInParent<Animator>();

        if (refAnimator == null)
        {
            Debug.LogWarning("Animator not found in parent");
        }
    }

    public void OnLanding()
    {
        refAnimator.SetTrigger("Landing");
    }

    // 상태 교체를 애니메이터에 알림.
    public void OnStateChanged(PlayerStateManager.State newState)
    {
        if (refAnimator == null)
        {
            Debug.LogError("refAnimator is null");
            return;
        }
        refAnimator.SetInteger("State", (int)newState);
    }

    // 점프에 대해서 0.03초 동안 부드럽게 넘어가고, 0.3초부터 시작
    public void PlayJump()
    {
        refAnimator.CrossFade("Jump", 0.03f, 0, 0.3f);
    }

    // 전투 모드
    public void SetCombat(bool isCombat)
    {
        if (refAnimator == null)
        {
            Debug.LogError("refAnimator is null");
            return;
        }
        refAnimator.SetBool("IsCombat", isCombat);
    }

    // 공격 상태 설정
    public void SetAttackTrigger()
    {
        if (refAnimator == null) return;
        refAnimator.SetTrigger("Attack");
    }

    // 콤보 공격 상태 설정
    public void SetNextAttackTrigger()
    {
        if (refAnimator == null) return;
        refAnimator.SetTrigger("NextAttack");
    }

    public AnimatorStateInfo GetCurrentStateInfo()
    {
        return refAnimator.GetCurrentAnimatorStateInfo(0);
    }

    public void PlayDie()
    {
        if (refAnimator == null) return;

        refAnimator.SetTrigger("Die");
    }
}
