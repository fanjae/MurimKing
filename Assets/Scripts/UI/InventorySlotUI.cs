using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler,  IEndDragHandler, IDropHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject selectedOutline;

    private InventoryUI owner;
    private ItemData currentItem;
    private int slotIndex;

    // 전체 슬롯 UI가 공유하는 현재 드래그 출발 슬롯
    private static InventorySlotUI draggedSlot;

    public int SlotIndex => slotIndex;

    // 슬롯 생성 후 인덱스와 소유 Inventory UI 연결.
    public void Init(int index, InventoryUI inventoryUI)
    {
        slotIndex = index;
        owner = inventoryUI;
    }

    // 슬롯의 아이템, 아이콘, 수량 상태 갱신
    public void Refresh(ItemData itemData, int count, bool selected)
    {
        currentItem = itemData;

        bool hasItem = itemData != null;

        // 빈 슬롯이면 아이콘 숨기고, Sprite NULL 처리.
        iconImage.enabled = hasItem;
        iconImage.sprite = hasItem ? itemData.Icon : null;

        // 아이템 2개 이상일때만 수량 표시
        countText.text = hasItem && count > 1
            ? count.ToString()
            : string.Empty;

        // 현재 선택된 슬롯이면 외곽선 표시
        selectedOutline.SetActive(selected);
    }

    // 슬롯 클릭 입력을 처리
    public void OnPointerClick(PointerEventData eventData)
    {
        if (owner == null) return;

        // 좌클릭 처리
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            owner.SelectSlot(slotIndex);
            return;
        }

        // 우 클릭 처리
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 빈 슬롯에서는 메뉴를 띄우지 않음
            if (currentItem == null) return;

            owner.ShowActionMenu(slotIndex, eventData.position);
        }
    }

    // 마우스 올리면 슬롯에서 툴팁 표시
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (owner == null) return;

        if (currentItem != null)
            owner.ShowTooltip(currentItem);
    }

    // 마우스 떼면 슬롯에서 툴팁 숨기기
    public void OnPointerExit(PointerEventData eventData)
    {
        if (owner == null) return;

        owner.HideTooltip();
    }

    // 슬롯 드래그 시작 처리
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 우클릭 메뉴와 충돌하지 않도록 좌클릭만 허용
        if (eventData.button != PointerEventData.InputButton.Left) return;

        // 빈 슬롯은 드래그 불가능
        if (currentItem == null) return;

        // 필요한 참조나 아이콘 없을 때 드래그 중단
        if (owner == null || iconImage.sprite == null) return;

        // 현재 슬롯을 드래그의 출발 슬롯으로 처리
        draggedSlot = this;

        // 드래그가 시작되면 기존 메뉴와 툴팁 숨김
        owner.HideTooltip();
        owner.HideActionMenu();
    }

    // Unity에서 드래그 이벤트를 유지하기 위해 필요
    // 현재는 별도의 드래그 아이콘을 사용하지 않으므로 내부 처리 없음
    public void OnDrag(PointerEventData eventData)
    {
        if (draggedSlot != this) return;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 종료 시 드래그 상태 정리
        if (draggedSlot != this) return;

        draggedSlot = null;
    }

    // 드래그한 아이템이 현재 슬롯 위에 드랍되면 출발 슬롯과 도착 슬롯 위치 교환
    public void OnDrop(PointerEventData eventData)
    {
        // 현재 드래그 중인 슬롯 없으면 처리 x
        if (draggedSlot == null) return;

        if (owner == null) return;

        int fromIndex = draggedSlot.slotIndex;
        int toIndex = slotIndex;

        // 동일한 슬롯에 드롭한 경우 교환 필요 없음.
        if (fromIndex == toIndex) return;

        owner.SwapSlots(fromIndex, toIndex);
    }

    private void OnDisable()
    {
        // 인벤토리를 닫는 도중 드래그 상태가 남는 것 방지
        if (draggedSlot == this)
        {
            draggedSlot = null;
        }
    }
}
