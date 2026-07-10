using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionMenuUI : MonoBehaviour
{
    // 화면 좌표를 Canvas 내부 좌표로 변환
    [SerializeField] private Canvas canvas;

    [Header("Buttons")]
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;

    private RectTransform rectTransform;

    private int selectedSlotIndex = -1;

    // 아이템 사용 요청에 대한 Action
    // 슬롯 인덱스 InventoryUI 전달
    private Action<int> onUseRequested;

    // 아이템 버리기 요청에 대한 Action
    // 슬롯 인덱스 InventoryUI 전달
    private Action<int> onDropRequested;

    private void Awake()
    {
        // 메뉴 위치 변경을 위해 RectTransform 가져오기
        rectTransform = GetComponent<RectTransform>();

        useButton.onClick.AddListener(UseSelectedItem);
        dropButton.onClick.AddListener(DropSelectedItem);

        gameObject.SetActive(false);
    }

    public void Show(int slotIndex,Vector2 screenPosition,Action<int> useCallback,Action<int> dropCallback)
    {
        // 현재 메뉴 대상 슬롯 저장 및 버튼 클릭시 호출할 외부 로직 저장.
        selectedSlotIndex = slotIndex;
        onUseRequested = useCallback;
        onDropRequested = dropCallback;

        gameObject.SetActive(true);
        SetPosition(screenPosition);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        // 이전 슬롯 정보 남지 않게 초기화
        selectedSlotIndex = -1;

        // 이전에 등록된 콜백 제거
        onUseRequested = null;
        onDropRequested = null;
    }

    // 선택된 아이템 사용
    private void UseSelectedItem()
    {
        // 유효한 슬롯 선택되지 않은 경우 처리하지 않음
        if (selectedSlotIndex < 0) return;

        // 콜백이 등록되어있다면 선택 슬롯 인덱스 전달
        onUseRequested?.Invoke(selectedSlotIndex);

        Hide();
    }

    // 선택된 아이템 버리기
    private void DropSelectedItem()
    {
        // 유효한 슬롯 선택되지 않은 경우 처리하지 않음.
        if (selectedSlotIndex < 0) return;

        // 콜백이 등록되어있다면 선택 슬롯 인덱스 전달
        onDropRequested?.Invoke(selectedSlotIndex);
        Hide();
    }

    private void SetPosition(Vector2 screenPosition)
    {
        RectTransform canvasRect = canvas.transform as RectTransform;

        // Canvas에 설정되어있는 카메라를 사용하도록 처리
        Camera eventCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        // 화면 좌표를 Canvas 내부 좌표로 변환하도록 처리
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,screenPosition,eventCamera,out Vector2 localPosition))
        {
            rectTransform.anchoredPosition = localPosition;
        }
    }
}