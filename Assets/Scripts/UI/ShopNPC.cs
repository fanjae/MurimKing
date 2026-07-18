using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    [Header("상점 데이터")]
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private ShopData shopData;
    [SerializeField] private ItemDatabase itemDatabase;

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerGoldController goldController;

    // 상점 구매 및 판매 로직 처리
    private ShopController shopController;

    // 플레이어 인벤토리 접근에 사용
    private PlayerInventoryController inventoryController;

    // 게임 초기화 과정에서 플레이어 인벤토리 컨트롤러 전달
    public void Initialize(PlayerInventoryController playerInventoryController)
    {
        // 전달받은 플레이어 인벤토리 컨트롤러 검사
        if (playerInventoryController == null)
        {
            Debug.LogError("PlayerInventoryController가 null입니다.", this);
            return;
        }

        // 상점 생성에 필요한 Inspector 참조 검사
        if (itemDatabase == null || goldController == null || shopData == null)
        {
            Debug.LogError("ShopController 생성에 필요한 데이터가 연결되지 않았습니다.", this);
            return;
        }

        inventoryController = playerInventoryController;

        // 아이템 데이터, 골드, 인벤토리, 상점 데이터를 이용해 컨트롤러 생성
        shopController = new ShopController(itemDatabase, goldController, inventoryController, shopData);
    }

    // 상점 UI 열기
    public void OpenShop()
    {
        // 상점 컨트롤러가 생성되지 않았다면 상점을 열지 않음
        if (shopController == null)
        {
            Debug.LogWarning("ShopController가 초기화되지 않았습니다.", this);
            return;
        }

        // 상점 UI가 Inspector에 연결되지 않은 경우 처리 중단
        if (shopUI == null)
        {
            Debug.LogWarning("ShopUI가 연결되지 않았습니다.", this);
            return;
        }

        // 상점 컨트롤러와 데이터를 전달해 상점 UI 열기
        shopUI.Open(shopController, shopData, itemDatabase);
    }

    // 상점 UI 닫기
    public void CloseShop()
    {
        // ShopUI가 연결되지 않은 경우 처리 중단
        if (shopUI == null)
        {
            Debug.LogWarning("ShopUI가 연결되지 않았습니다.", this);
            return;
        }

        shopUI.Close();
    }
}