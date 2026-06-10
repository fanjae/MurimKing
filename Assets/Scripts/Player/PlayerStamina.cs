using UnityEngine;
using UnityEngine.UI;


public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float recoverSpeed = 10.0f;
    [SerializeField] private Image staminaGage;

    private float currentStamina;
    private PlayerStateManager stateManager;

    private void Awake()
    {
        stateManager = GetComponent<PlayerStateManager>();

        currentStamina = maxStamina;
        UpdateStaminaGage();
    }

    private void Update()
    {
        if (stateManager != null && stateManager.IsAttackingNow())
        {
            return;
        }

        RecoverStamina();
    }

    private void RecoverStamina() // 스태미나가 일정 시간마다 회복되도록 처리
    {
        if (currentStamina >= maxStamina) return;

        currentStamina += recoverSpeed * Time.deltaTime; 
        currentStamina = Mathf.Clamp(currentStamina, 0.0f, maxStamina);

        UpdateStaminaGage();
    }

    public bool CanUse(float amount)
    {
        return currentStamina >= amount;
    }

    public bool TryUse(float amount) // 스태미나 사용하는 경우 시도
    {
        if (currentStamina < amount) return false;

        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0.0f, maxStamina);

        UpdateStaminaGage();
        return true;
    }

    private void UpdateStaminaGage()
    {
        if (staminaGage == null) return;

        staminaGage.fillAmount = currentStamina / maxStamina;
    }
}
