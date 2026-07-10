using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI[] slotUIs;
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private ItemTooltipUI tooltipUI;
    [SerializeField] private ItemActionMenuUI actionMenuUI;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Canvas rootCanvas;

    private Inventory inventory;
    private PlayerInventoryController inventoryController;
    private int selectedIndex = -1;

    public Canvas RootCanvas => rootCanvas;

    public void Init(Inventory newInventory)
    {
        // 기존 이벤트 제거
        if (inventory != null) inventory.OnInventoryChanged -= Refresh;

        inventory = newInventory;

        // 인벤토리 조작을 담당하는 컨트롤러 생성
        inventoryController = new PlayerInventoryController(inventory,itemDatabase);

        // 모든 슬롯 UI 초기화
        for (int i = 0; i < slotUIs.Length; i++)
        {
            if (slotUIs[i] == null)
            {
                Debug.LogError($"slotUIs[{i}]가 연결되지 않았습니다.");
                continue;
            }

            slotUIs[i].Init(i, this);
        }

        // 인벤토리 변경 시 UI 자동 갱신
        inventory.OnInventoryChanged += Refresh;
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
    }

    // 우클릭 메뉴 표시
    public void ShowActionMenu(int slotIndex, Vector2 screenPosition)
    {
        if (inventory == null) return;

        // 슬롯 확인
        if (!inventory.TryGetSlot(slotIndex, out InventorySlot slot)) return;

        // 빈 슬롯은 메뉴 띄우지 않음
        if (slot.IsEmpty) return;

        // 선택 슬롯 변경
        selectedIndex = slotIndex;
        Refresh();
        
        // 메뉴 생성 및 버튼 이벤트 연결
        actionMenuUI.Show(slotIndex,screenPosition,UseItem,DropItem);
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
}