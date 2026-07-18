using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ShopData",
    menuName = "Shop/Shop Data")]
public class ShopData : ScriptableObject
{
    [SerializeField]
    private List<ShopItemData> shopItems = new();

    [SerializeField]
    [Range(0f, 1f)]
    private float sellPriceRate = 0.5f;

    public IReadOnlyList<ShopItemData> ShopItems => shopItems;
    public float SellPriceRate => sellPriceRate;
}