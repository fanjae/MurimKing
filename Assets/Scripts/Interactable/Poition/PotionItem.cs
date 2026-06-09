using UnityEngine;

public class PotionItem : MonoBehaviour
{
    [SerializeField] private float healAmount = 50.0f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>(); // 플레이어 체력 회복 오브젝트 보유시

        if (playerHealth == null) return;

        playerHealth.Heal(healAmount); // 회복

        Destroy(gameObject); // 오브젝트 파괴
    }
}