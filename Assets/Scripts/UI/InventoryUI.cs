using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Gold UI")]
    [SerializeField] private PlayerGoldController goldController;
    [SerializeField] private TMP_Text goldText;

    [SerializeField] private InventorySlotUI[] slotUIs;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private ItemTooltipUI tooltipUI;
    [SerializeField] private ItemActionMenuUI actionMenuUI;

    [Header("Shop")]
    [SerializeField] private ShopUI shopUI;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Canvas rootCanvas;

    private Inventory inventory;
    private PlayerInventoryController inventoryController;
    private int selectedIndex = -1;

    public Canvas RootCanvas => rootCanvas;

    public void Init(
    Inventory newInventory,
    PlayerInventoryController newInventoryController,
    ItemDatabase newItemDatabase)
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= Refresh;
        }

        // 중복 구독 방지
        if (goldController != null)
        {
            goldController.OnGoldChanged -= RefreshGold;
        }

        inventory = newInventory;
        inventoryController = newInventoryController;
        itemDatabase = newItemDatabase;

        if (inventory == null)
        {
            Debug.LogError("Inventory가 null입니다.");
            return;
        }

        if (inventoryController == null)
        {
            Debug.LogError("PlayerInventoryController가 null입니다.");
            return;
        }

        if (itemDatabase == null)
        {
            Debug.LogError("ItemDatabase가 null입니다.");
            return;
        }

        if (goldController == null)
        {
            Debug.LogError("PlayerGoldController가 연결되지 않았습니다.");
        }

        if (goldText == null)
        {
            Debug.LogError("Gold Text가 연결되지 않았습니다.");
        }

        for (int i = 0; i < slotUIs.Length; i++)
        {
            if (slotUIs[i] == null)
            {
                Debug.LogError($"slotUIs[{i}]가 연결되지 않았습니다.");
                continue;
            }

            slotUIs[i].Init(i, this);
        }

        inventory.OnInventoryChanged += Refresh;

        if (goldController != null)
        {
            goldController.OnGoldChanged += RefreshGold;

            // Awake에서 발생한 최초 이벤트를 놓칠 수 있으므로 직접 초기화
            RefreshGold(goldController.Gold);
        }

        Refresh();
    }

    // 슬롯 선택
    public void SelectSlot(int index)
    {
        selectedIndex = index;
        Refresh();
    }

    // 아이템 툴팁 표시
    public void ShowTooltip(ItemData itemData)
    {
        tooltipUI.Show(itemData);
    }

    // 아이템 툴팁 숨김
    public void HideTooltip()
    {
        tooltipUI.Hide();
    }

    // 우클릭 메뉴 숨김
    public void HideActionMenu()
    {
        if (actionMenuUI == null) return;

        actionMenuUI.Hide();
    }

    // 인벤토리 데이터를 기반으로 슬롯 UI 갱신
    private void Refresh()
    {
        if (inventory == null) return;

        for (int i = 0; i < slotUIs.Length; i++)
        {
            InventorySlotUI slotUI = slotUIs[i];

            if (slotUI == null)
                continue;

            // 존재하지 않는 슬롯 빈 슬롯으로 표시
            if (!inventory.TryGetSlot(i, out InventorySlot slot))
            {
                slotUI.Refresh(null, 0, false);
                continue;
            }

            // ItemId를 이용해 실제 아이템 데이터 조회
            ItemData itemData = slot.IsEmpty ? null : itemDatabase.GetItem(slot.ItemId);

            // 슬롯 UI 갱신
            slotUI.Refresh(itemData,slot.Count,i == selectedIndex);
        }
    }

    // UII 비활성화되면 열려있는 UI 모두 닫음
    private void OnDisable()
    {
        HideTooltip();
        HideActionMenu();
    }

    // 이벤트 구독 해제
    private void OnDestroy()
    {
        if (inventory != null) inventory.OnInventoryChanged -= Refresh;

        if (goldController != null) goldController.OnGoldChanged -= RefreshGold;
    }

    // 우클릭 메뉴 표시
    public void ShowActionMenu(int slotIndex, Vector2 screenPosition)
    {
        if (inventory == null) return;

        // 슬롯 확인
        // if (!inventory.TryGetSlot(slotIndex, out InventorySlot slot)) return;
        if (!inventoryController.TryGetInventorySlot(slotIndex, out InventorySlot slot)) return;

        // 빈 슬롯은 메뉴 띄우지 않음
        if (slot.IsEmpty) return;

        // 선택 슬롯 변경
        selectedIndex = slotIndex;
        Refresh();

        // 메뉴 생성 및 버튼 이벤트 연결
        bool canSell = shopUI != null && shopUI.IsOpen;
        actionMenuUI.Show(slotIndex, screenPosition, UseItem, DropItem, SellItem, canSell);
    }

    // 선택한 아이템 사용
    public void UseItem(int slotIndex)
    {
        if (inventoryController == null) return;

        bool used = inventoryController.UseConsumableFromInventory(slotIndex,playerHealth);
        if (!used)
        {
            Debug.Log("아이템을 사용할 수 없습니다. " + "소비 아이템이 아니거나 HP가 가득 찼습니다." );
            return;
        }

        // 사용 후 선택 해제
        selectedIndex = -1;
    }

    // 아이템 한 개 버리기
    private void DropItem(int slotIndex)
    {
        if (inventoryController == null)
            return;

        bool removed = inventoryController.RemoveFromInventory(slotIndex, 1);

        if (!removed)  return;

        selectedIndex = -1;

        Debug.Log("아이템을 버렸습니다.");
    }

    // 슬롯 간 아이템 위치 교환
    public void SwapSlots(int fromIndex, int toIndex)
    {
        if (inventoryController == null)  return;

        bool swapped = inventoryController.SwapInventorySlots(fromIndex, toIndex);

        if (!swapped)
        {
            Debug.LogWarning($"인벤토리 슬롯 교환 실패: {fromIndex} -> {toIndex}");
        }
    }

    private void RefreshGold(int gold)
    {
        if (goldText == null)
            return;

        goldText.text = $"GOLD : {gold}G";
    }

    // 선택한 아이템 한 개 판매
    private void SellItem(int slotIndex)
    {
        if (shopUI == null || !shopUI.IsOpen)
        {
            Debug.LogWarning("열려 있는 상점이 없습니다.");
            return;
        }

        bool sold = shopUI.SellItemAt(slotIndex, 1);

        if (!sold)
        {
            Debug.LogWarning("아이템 판매에 실패했습니다.");
            return;
        }

        selectedIndex = -1;

        Debug.Log("아이템을 판매했습니다.");
    }
}