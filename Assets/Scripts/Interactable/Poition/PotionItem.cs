using UnityEngine;

public class PotionItem : MonoBehaviour
{
    [SerializeField] private PotionData potionData;
    private bool isUsed; // 중복 사용 방지

    private void OnTriggerEnter(Collider other)
    {
        // 중복 사용 방지 및 포션 데이터 없으면 미처리
        if (isUsed || potionData == null) return;

        // 충돌한 오브젝트 또는 부모 오브젝트에서 PlayerHealth를 찾아서 처리한다.
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null) return;

        isUsed = true;

        // 포션 데이터에 저장된 회복량만큼 플레이어 체력을 회복
        playerHealth.Heal(potionData.healAmount);

        // 플레이어 오브젝트에 AudioSource가 있으면 포션 사운드 재생
        AudioSource playerAudio = playerHealth.GetComponent<AudioSource>();
        if (playerAudio != null && potionData.useSound != null)
        {
            playerAudio.PlayOneShot(potionData.useSound);
        }

        // 오브젝트 제거
        Destroy(gameObject);
    }
}