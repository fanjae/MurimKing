using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemData",menuName = "Shop/Shop Item Data")]
public class ShopItemData : ScriptableObject
{
    [SerializeField] private int itemId;
    [SerializeField] private int buyPrice;

    public int ItemId => itemId;
    public int BuyPrice => buyPrice;
}