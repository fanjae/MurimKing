using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;

    private ItemData itemData;
    private ShopItemData shopItemData;
    private ShopUI shopUI;

    public void Initialize(
        ItemData newItemData,
        ShopItemData newShopItemData,
        ShopUI ownerShopUI)
    {
        if (newItemData == null ||
            newShopItemData == null ||
            ownerShopUI == null)
        {
            Debug.LogWarning("ShopItemUI 초기화에 필요한 데이터가 없습니다.");
            return;
        }

        itemData = newItemData;
        shopItemData = newShopItemData;
        shopUI = ownerShopUI;

        icon.sprite = itemData.Icon;
        nameText.text = itemData.ItemName;
        priceText.text = $"{shopItemData.BuyPrice} G";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        if (shopUI == null ||
            itemData == null ||
            shopItemData == null)
        {
            return;
        }

        shopUI.OpenPurchasePopup(
            itemData,
            shopItemData
        );
    }
}