using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 10.0f;

    private void OnTriggerStay(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null) 
        {
            Debug.LogWarning("PlayerHealth not found");
            return;
        }

        playerHealth.TakeDamage(damagePerSecond * Time.deltaTime); // 플레이어에게 데미지 준다.
    }
}