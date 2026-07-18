using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPurchasePopupUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text totalPriceText;

    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button cancelButton;

    [Header("구매 설정")]
    [SerializeField] private int maxPurchaseQuantity = 99;

    private ShopController shopController;

    private ShopUI shopUI;

    // 현재 구매 대상으로 선택된 실제 아이템 데이터
    private ItemData currentItemData;

    // 현재 구매 대상으로 선택된 상점 아이템 데이터
    private ShopItemData currentShopItemData;

    // 현재 선택한 구매 수량
    private int quantity = 1;

    private void Awake()
    {
        // UI 참조가 모두 연결되어 있는지 확인
        if (decreaseButton == null || increaseButton == null || buyButton == null || cancelButton == null)
        {
            Debug.LogError("구매 팝업 버튼이 모두 연결되지 않았습니다.", this);
            return;
        }

        // 각 버튼에 기능 등록
        decreaseButton.onClick.AddListener(DecreaseQuantity);
        increaseButton.onClick.AddListener(IncreaseQuantity);
        buyButton.onClick.AddListener(ConfirmPurchase);
        cancelButton.onClick.AddListener(Close);

        // 게임 시작 시 구매 팝업 숨김
        gameObject.SetActive(false);
    }

    // 선택한 상품 정보를 전달받아 구매 팝업 표시
    public void Open(ItemData itemData, ShopItemData shopItemData, ShopController controller, ShopUI ownerShopUI)
    {
        // 팝업을 여는 데 필요한 데이터가 하나라도 없으면 처리 중단
        if (itemData == null || shopItemData == null || controller == null || ownerShopUI == null)
        {
            Debug.LogWarning("구매 팝업을 여는 데 필요한 데이터가 없습니다.", this);
            return;
        }

        // 선택한 상품과 구매 처리에 필요한 참조 저장
        currentItemData = itemData;
        currentShopItemData = shopItemData;
        shopController = controller;
        shopUI = ownerShopUI;

        // 팝업을 열 때 구매 수량을 1개로 초기화
        quantity = 1;

        // 팝업을 먼저 활성화한 뒤 UI 내용 갱신
        gameObject.SetActive(true);
        Refresh();
    }

    // 구매 팝업을 닫고 이전 상품 정보 초기화
    public void Close()
    {
        gameObject.SetActive(false);

        // 이전 상품 및 상점 참조가 남지 않도록 초기화
        currentItemData = null;
        currentShopItemData = null;
        shopController = null;
        shopUI = null;

        quantity = 1;
    }

    // 구매 수량 1 증가
    private void IncreaseQuantity()
    {
        // 설정된 최대 구매 수량을 넘지 않도록 제한
        quantity = Mathf.Min(quantity + 1, maxPurchaseQuantity);

        Refresh();
    }

    // 구매 수량 1 감소
    private void DecreaseQuantity()
    {
        // 구매 수량이 최소 1보다 작아지지 않도록 제한
        quantity = Mathf.Max(quantity - 1, 1);

        Refresh();
    }

    // 현재 선택한 수량만큼 실제 구매 요청
    private void ConfirmPurchase()
    {
        // 구매에 필요한 컨트롤러나 상품 데이터가 없으면 처리 중단
        if (shopController == null || currentShopItemData == null) return;

        // 선택한 ItemId와 수량으로 구매 처리
        PurchaseResult result = shopController.BuyItem(currentShopItemData.ItemId, quantity);

        // 구매 성공 또는 실패 결과를 상점 UI에 표시
        if (shopUI != null)
        {
            shopUI.ShowPurchaseResult(result);
        }

        // 구매 성공 시 팝업 닫기
        if (result == PurchaseResult.Success)
        {
            Close();
        }
    }

    // 현재 선택한 상품과 수량을 기준으로 팝업 UI 갱신
    private void Refresh()
    {
        // 표시할 상품 데이터가 없으면 갱신 중단
        if (currentItemData == null || currentShopItemData == null) return;

        // 현재 상품 이름과 선택 수량 표시
        if (itemNameText != null)
        {
            itemNameText.text = currentItemData.ItemName;
        }

        if (quantityText != null)
        {
            quantityText.text = quantity.ToString();
        }

        // 선택 수량을 반영한 총 구매 가격 계산
        int totalPrice = currentShopItemData.BuyPrice * quantity;

        if (totalPriceText != null)
        {
            totalPriceText.text = $"Total Price : {totalPrice}G";
        }

        // 최소 및 최대 수량에서는 해당 방향 버튼 비활성화
        if (decreaseButton != null)
        {
            decreaseButton.interactable = quantity > 1;
        }

        if (increaseButton != null)
        {
            increaseButton.interactable = quantity < maxPurchaseQuantity;
        }
    }

    private void OnDestroy()
    {
        // 오브젝트 파괴 시 버튼 이벤트 구독 해제
        if (decreaseButton != null)
        {
            decreaseButton.onClick.RemoveListener(DecreaseQuantity);
        }

        if (increaseButton != null)
        {
            increaseButton.onClick.RemoveListener(IncreaseQuantity);
        }

        if (buyButton != null)
        {
            buyButton.onClick.RemoveListener(ConfirmPurchase);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveListener(Close);
        }
    }
}