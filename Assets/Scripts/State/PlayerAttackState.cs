using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackState : PlayerStateBase
{
    //공격 판정을 시작할때 발행하는 이벤트
    [SerializeField] private UnityEvent OnAttackBegin;
    //공격 판정을 종료할때 발행할 이벤트
    [SerializeField] private UnityEvent OnAttackCheckEnd;
    //공격을 종료할때 발행할 이벤트
    [SerializeField] private UnityEvent OnAttackEnd;

    //애니메이션 이벤트 리스너 메서드
    private void AttackStart()
    {
        if (!manager.UseAttackStamina())
        {
            return;
        }

        OnAttackBegin?.Invoke();
    }
    //공격 시작 이벤트에 리스너 메서드를 등록할때 사용하는 메서드
    public void SubscribeOnAttackBegin(UnityAction listener)
    {
        OnAttackBegin?.AddListener(listener);
    }
    //공격 애니메이션에서 AttackCheckEnd 이벤트가 발생할때 실행
    private void AttackCheckEnd()
    {
        //공격 판정 종료 이벤트
        OnAttackCheckEnd?.Invoke();
    }
    //공격 판정 종료 이벤트에 리스너 메서드를 등록 할때 사용하는 메서드
    public void SubscribeOnAttackCheckEnd(UnityAction listener)
    {
        OnAttackCheckEnd?.AddListener(listener);
    }
    private void ComboCheck()
    {
        if (manager.IsNextAttackReserved)
        {
            animationController.SetNextAttackTrigger();
            manager.ClearNextAttackReserved();
        }
    }
    //공격 애니메이션에서 AttackEnd 이벤트가 발생할때 실행
    private void AttackEnd()
    {
        //공격 판정 종료 이벤트
        OnAttackEnd?.Invoke();
        manager.ClearNextAttackReserved();
    }
    //공격 종료 이벤트에 리스너 메서드를 등록할때 사용하는 메서드
    public void SubscribeOnAttackEnd(UnityAction listener)
    {
        //OnAttackEnd 이벤트에 구독할 메서드 추가
        OnAttackEnd?.AddListener(listener);
    }
}
