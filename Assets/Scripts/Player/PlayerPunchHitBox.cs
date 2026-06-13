using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchHitbox : MonoBehaviour
{
    // 오른쪽 공격 판정 위치, 왼손 공격 판정 위치
    [SerializeField] private Transform rightPunchPoint;
    [SerializeField] private Transform leftPunchPoint;

    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float damage = 10f;

    private Transform currentPunchPoint;

    private bool isChecking;

    // 이미 맞은 적에게 중복으로 데미지가 들어가지 않도록 맞은적 저장
    private readonly HashSet<Collider> damagedTargets = new();

    private void Update()
    {
        // 공격 중이 아니거나 판정 위치 없으면 무효
        if (!isChecking || currentPunchPoint == null)
            return;

        Collider[] hits = Physics.OverlapSphere(
            currentPunchPoint.position,
            radius,
            enemyLayer
        );

        foreach (Collider hit in hits)
        {
            // 이미 데미지를 준 대상 건너 뛰기
            if (damagedTargets.Contains(hit))
                continue;

            damagedTargets.Add(hit);

            // 부모 오브젝트에서 EnemyHealth 탐색
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                // 적에게 데미지 적용
                enemy.TakeDamage(damage);
            }
        }
    }

    // 공격 시작 시 호출
    public void AttackBegin(Transform punchPoint)
    {
        // 현재 공격에 사용할 판정 위치 지정
        currentPunchPoint = punchPoint;
        damagedTargets.Clear();
        isChecking = true;
    }

    // 공격 종료 시 호출
    public void AttackEnd()
    {

        // 판정 해제
        isChecking = false;
        currentPunchPoint = null;
    }

    // 공격 범위 표시
    private void OnDrawGizmos()
    {
        if (rightPunchPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(rightPunchPoint.position, radius);
        }

        if (leftPunchPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(leftPunchPoint.position, radius);
        }

        if (currentPunchPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentPunchPoint.position, radius);
        }
    }
}