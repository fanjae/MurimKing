using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 100.0f;
    [SerializeField] private Image hpGage;

    private float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        UpdateHpGage(); 
    }

    public void Heal(float amount)
    {
        if (currentHP <= 0.0f) return; // -인 경우 유효한 값이 아님

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0.0f, maxHP); // 0~maxHP 범위까지

        UpdateHpGage(); 
    }
    public void TakeDamage(float damage) // 데미지 입음
    {
        if (currentHP <= 0.0f) return;

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0.0f, maxHP); // 0~maxHP 범위까지

        UpdateHpGage();

        if (currentHP <= 0.0f) // 사망 처리
        {
            // Die
        }
    }

    private void UpdateHpGage() // HP 게이지 업데이트
    {
        if (hpGage == null) return;

        hpGage.fillAmount = currentHP / maxHP;
    }
}