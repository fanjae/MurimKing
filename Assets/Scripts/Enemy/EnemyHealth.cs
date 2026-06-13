using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyData data;

    private float currentHP;
    private bool isDead;

    private void Awake()
    {
        // 게임 시작 시 EnemyData에 저장된 최대 체력으로 현재 체력 초기화
        currentHP = data.maxHP;
    }

    public void TakeDamage(float damage)
    {
        // 이미 사망한 적이면 더 이상 데미지를 받지 않음
        if (isDead) return;

        // 받은 데미지 만큼 현재 체력 감소
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, data.maxHP);

        // 체력이 0이하가 되면 사망 처리
        if (currentHP <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Destroy(gameObject, 1f);
    }
}