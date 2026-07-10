using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 100.0f;
    [SerializeField] private Image hpGage;

    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 5.0f;

    private float currentHP;
    private bool isDead = false;

    private PlayerStateManager stateManager;
    private CharacterController characterController;
    private Animator animator;

    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;
    public bool IsDead => isDead;
    public bool CanHeal => !isDead && currentHP > 0f && currentHP < maxHP;

    private void Awake()
    {
        stateManager = GetComponent<PlayerStateManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        currentHP = maxHP;
        UpdateHpGage(); 
    }

    public bool Heal(float amount)
    {
        if (amount <= 0f) return false;

        if (!CanHeal) return false;

        float previousHP = currentHP;

        currentHP = Mathf.Clamp(currentHP + amount,0f,maxHP);

        UpdateHpGage();

        return currentHP > previousHP;
    }
    public void TakeDamage(float damage) // 데미지 입음
    {
        if (isDead) return; // 사망한 경우 명령 무시

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0.0f, maxHP); // 0~maxHP 범위까지

        UpdateHpGage();

        if (currentHP <= 0.0f) // 사망 처리
        {
            StartCoroutine(DieRoutine());
        }
    }
    private IEnumerator DieRoutine()
    {
        isDead = true;

        // 사망
        stateManager.SetState(PlayerStateManager.State.Die);

        yield return new WaitForSeconds(respawnDelay);

        // 체력 복구
        currentHP = maxHP;
        UpdateHpGage();

        // 부활
        Respawn();

        isDead = false;
    }


    private void UpdateHpGage() // HP 게이지 업데이트
    {
        if (hpGage == null) return;

        hpGage.fillAmount = currentHP / maxHP;
    }

    private void Respawn()
    {
        if (respawnPoint == null) return;

        // CharacterController 활성 상태에서 Transform 이동 시 충돌 문제를 해결하기 위해 비활성화
        if (characterController != null)
            characterController.enabled = false;

        // 리스폰 위치 및 회전 적용
        transform.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        if (characterController != null)
            characterController.enabled = true;

        // IDLE 복구
        stateManager.SetState(PlayerStateManager.State.Idle);
    }
}