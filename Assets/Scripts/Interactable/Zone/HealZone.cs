using UnityEngine;

public class HealZone : MonoBehaviour
{
    [SerializeField] private float healPerSecond = 10.0f;

    private void OnTriggerStay(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null) return;

        playerHealth.Heal(healPerSecond * Time.deltaTime); // 플레이어 체력 회복
    }
}