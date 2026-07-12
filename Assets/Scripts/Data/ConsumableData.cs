using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableData : ItemData
{
    // 소비 아이템
    // 타입(순수 회복)
    // 회복 값

    [Header("Consumable")]
    [SerializeField] private ConsumableType consumableType;
    [SerializeField] private int value;

    public ConsumableType ConsuabmeType => consumableType;
    public int Value => value;

    public override bool Use(PlayerHealth playerHealth)
    {
        if (playerHealth == null) return false;

        switch (consumableType)
        {
            case ConsumableType.HealHp: // 체력 포션 사용
                return playerHealth.Heal(value);
                
            default:
                return false;
        }
    }
}
