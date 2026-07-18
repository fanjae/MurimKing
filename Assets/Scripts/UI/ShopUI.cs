using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform itemContent;
    [SerializeField] private ShopItemUI shopItemPrefab;
    [SerializeField] private ScrollRect itemScrollRect;
    [SerializeField] private TMP_Text messageText;

    [Header("Purchase Popup")]
    [SerializeField] private ShopPurchasePopupUI purchasePopup;

    [Header("Message")]
    [SerializeField] private float messageDisplayTime = 3f;

    // 상점 동작에 필요한 데이터 참조
    private ShopController shopController;
    private ShopData currentShopData;
    private ItemDatabase itemDatabase;

    // 구매 결과 메시지를 일정 시간 뒤 제거하기 위한 코루틴
    private Coroutine messageCoroutine;

    // 현재 상점 UI와 ShopController가 활성화되어 있는지 확인
    public bool IsOpen => shopPanel != null && shopPanel.activeSelf && shopController != null;

    private void Awake()
    {
        // 게임 시작 시 상점 패널 비활성화
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }

        // 초기 메시지 제거
        ClearMessage();
    }

    // 상점 UI를 열고 필요한 데이터를 설정
    public void Open(ShopController controller, ShopData shopData, ItemDatabase database)
    {
        // 상점을 여는 데 필요한 데이터가 하나라도 없으면 중단
        if (controller == null || shopData == null || database == null)
        {
            Debug.LogWarning("상점 UI를 여는 데 필요한 데이터가 없습니다.");
            return;
        }

        shopController = controller;
        currentShopData = shopData;
        itemDatabase = database;

        // 이전 구매 결과 메시지와 제거 타이머 초기화
        StopMessageCoroutine();
        ClearMessage();

        // 상점 패널 활성화
        shopPanel.SetActive(true);

        // 현재 상점 데이터에 맞게 상품 목록 생성
        CreateShopItems();

        // UI 레이아웃을 즉시 갱신
        Canvas.ForceUpdateCanvases();

        // 상품 목록을 가장 위로 이동
        if (itemScrollRect != null)
        {
            itemScrollRect.verticalNormalizedPosition = 1f;
        }
    }

    // 상점 UI와 열려 있는 구매 팝업 닫기
    public void Close()
    {
        // 실행 중인 메시지 제거 코루틴 중단
        StopMessageCoroutine();

        // 구매 수량 선택 팝업이 열려 있다면 닫기
        if (purchasePopup != null)
        {
            purchasePopup.Close();
        }

        // 구매 결과 메시지 제거
        ClearMessage();

        // 상점 패널 비활성화
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }

        // 동적으로 생성된 상품 UI 제거
        ClearShopItems();

        // 이전 상점 데이터가 남지 않도록 참조 초기화
        shopController = null;
        currentShopData = null;
        itemDatabase = null;
    }

    // 상품 UI에서 구매 버튼을 누르면 구매 수량 선택 팝업 표시
    public void OpenPurchasePopup(ItemData itemData, ShopItemData shopItemData)
    {
        // 구매 팝업이 Inspector에 연결되지 않은 경우 처리 중단
        if (purchasePopup == null)
        {
            Debug.LogWarning("구매 팝업이 연결되지 않았습니다.");
            return;
        }

        // 상점 컨트롤러가 아직 초기화되지 않은 경우 처리 중단
        if (shopController == null)
        {
            Debug.LogWarning("ShopController가 초기화되지 않았습니다.");
            return;
        }

        // 팝업에 전달할 아이템 데이터 검사
        if (itemData == null || shopItemData == null)
        {
            Debug.LogWarning("구매 팝업에 전달할 아이템 데이터가 없습니다.");
            return;
        }

        // 선택한 아이템 정보와 상점 참조를 팝업에 전달
        purchasePopup.Open(itemData, shopItemData, shopController, this);
    }

    // 구매 결과에 맞는 메시지를 출력
    public void ShowPurchaseResult(PurchaseResult result)
    {
        if (messageText == null) return;

        switch (result)
        {
            case PurchaseResult.Success:
                messageText.text = "Success";
                break;

            case PurchaseResult.NotEnoughGold:
                messageText.text = "Not Enough Gold";
                break;

            case PurchaseResult.InventoryFull:
                messageText.text = "Inventory Full";
                break;

            case PurchaseResult.ItemNotFound:
                messageText.text = "Item Not Found";
                break;

            case PurchaseResult.InvalidRequest:
                messageText.text = "Invalid Request";
                break;

            default:
                messageText.text = "Can't Buy";
                break;
        }

        // 기존 제거 타이머가 실행 중이면 중단
        StopMessageCoroutine();

        // 마지막 메시지가 출력된 시점부터 다시 제거 시간 측정
        messageCoroutine = StartCoroutine(ClearMessageAfterDelay());
    }

    // 설정된 시간만큼 기다린 후 구매 결과 메시지 제거
    private IEnumerator ClearMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDisplayTime);

        ClearMessage();
        messageCoroutine = null;
    }

    // 현재 실행 중인 메시지 제거 코루틴 중단
    private void StopMessageCoroutine()
    {
        if (messageCoroutine == null) return;

        StopCoroutine(messageCoroutine);
        messageCoroutine = null;
    }

    // 구매 결과 텍스트 초기화
    private void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = string.Empty;
        }
    }

    // 현재 상점 데이터에 맞게 상품 UI 생성
    private void CreateShopItems()
    {
        // 기존 상품 UI를 먼저 제거
        ClearShopItems();

        // 상점 데이터와 상품 목록 유효성 검사
        if (currentShopData == null || currentShopData.ShopItems == null)
        {
            Debug.LogWarning("상점 상품 데이터가 없습니다.");
            return;
        }

        // 아이템 데이터베이스 유효성 검사
        if (itemDatabase == null)
        {
            Debug.LogWarning("ItemDatabase가 없습니다.");
            return;
        }

        // 상품 UI 생성에 필요한 참조 검사
        if (shopItemPrefab == null || itemContent == null)
        {
            Debug.LogWarning("상점 상품 UI 생성에 필요한 참조가 없습니다.");
            return;
        }

        // 상점에 등록된 상품 정보를 순회
        foreach (ShopItemData shopItemData in currentShopData.ShopItems)
        {
            if (shopItemData == null) continue;

            // ItemId를 이용해 실제 아이템 데이터 조회
            ItemData itemData = itemDatabase.GetItem(shopItemData.ItemId);

            // 데이터베이스에 없는 아이템은 생성하지 않음
            if (itemData == null)
            {
                Debug.LogWarning($"ItemId {shopItemData.ItemId}에 해당하는 ItemData가 없습니다.");
                continue;
            }

            // 상품 UI 프리팹 생성
            ShopItemUI shopItemUI = Instantiate(shopItemPrefab, itemContent);

            // 상품 정보와 현재 ShopUI 전달
            shopItemUI.Initialize(itemData, shopItemData, this);
        }

        // 생성된 상품 개수에 맞게 Content 크기 즉시 갱신
        if (itemContent is RectTransform contentRect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        }
    }

    // 동적으로 생성된 모든 상품 UI 제거
    private void ClearShopItems()
    {
        if (itemContent == null) return;

        // 자식 제거 중 인덱스가 변하지 않도록 뒤에서부터 제거
        for (int i = itemContent.childCount - 1; i >= 0; i--)
        {
            Destroy(itemContent.GetChild(i).gameObject);
        }
    }

    // 오브젝트가 비활성화될 때 메시지 관련 상태 정리
    private void OnDisable()
    {
        StopMessageCoroutine();
        ClearMessage();
    }

    // 현재 열려 있는 상점에 인벤토리 아이템 판매 요청
    public bool SellItemAt(int slotIndex, int count = 1)
    {
        if (!IsOpen) return false;

        return shopController.SellItemAt(slotIndex, count);
    }
}