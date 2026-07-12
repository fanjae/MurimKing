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
    [SerializeField] private Button sellButton;

    private RectTransform rectTransform;
    private int selectedSlotIndex = -1;

    // 아이템 사용 요청과 슬롯 인덱스 전달
    private Action<int> onUseRequested;

    // 아이템 버리기 요청과 슬롯 인덱스 전달
    private Action<int> onDropRequested;

    // 아이템 판매 요청과 슬롯 인덱스 전달
    private Action<int> onSellRequested;

    private void Awake()
    {
        // 메뉴 위치 변경을 위해 RectTransform 가져오기
        rectTransform = GetComponent<RectTransform>();

        // 버튼 클릭 이벤트 등록
        if (useButton != null) useButton.onClick.AddListener(UseSelectedItem);
        if (dropButton != null) dropButton.onClick.AddListener(DropSelectedItem);
        if (sellButton != null) sellButton.onClick.AddListener(SellSelectedItem);

        // 게임 시작 시 메뉴 숨김
        gameObject.SetActive(false);
    }

    // 선택 슬롯을 기준으로 액션 메뉴 표시
    public void Show(
        int slotIndex,
        Vector2 screenPosition,
        Action<int> useCallback,
        Action<int> dropCallback,
        Action<int> sellCallback,
        bool canSell)
    {
        // 현재 메뉴 대상 슬롯과 각 동작의 콜백 저장
        selectedSlotIndex = slotIndex;
        onUseRequested = useCallback;
        onDropRequested = dropCallback;
        onSellRequested = sellCallback;

        // 상점이 열려 있을 때만 판매 버튼 활성화
        if (sellButton != null)
        {
            sellButton.gameObject.SetActive(canSell);
        }

        gameObject.SetActive(true);
        SetPosition(screenPosition);
    }

    // 액션 메뉴 숨김 및 이전 상태 초기화
    public void Hide()
    {
        gameObject.SetActive(false);

        selectedSlotIndex = -1;

        onUseRequested = null;
        onDropRequested = null;
        onSellRequested = null;
    }

    // 선택한 아이템 사용 요청
    private void UseSelectedItem()
    {
        if (selectedSlotIndex < 0) return;

        onUseRequested?.Invoke(selectedSlotIndex);
        Hide();
    }

    // 선택한 아이템 버리기 요청
    private void DropSelectedItem()
    {
        if (selectedSlotIndex < 0) return;

        onDropRequested?.Invoke(selectedSlotIndex);
        Hide();
    }

    // 선택한 아이템 판매 요청
    private void SellSelectedItem()
    {
        if (selectedSlotIndex < 0) return;

        onSellRequested?.Invoke(selectedSlotIndex);
        Hide();
    }

    // 마우스 화면 좌표를 Canvas 내부 좌표로 변환해 메뉴 위치 지정
    private void SetPosition(Vector2 screenPosition)
    {
        if (canvas == null || rectTransform == null) return;

        RectTransform canvasRect = canvas.transform as RectTransform;
        Camera eventCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, eventCamera, out Vector2 localPosition))
        {
            rectTransform.anchoredPosition = localPosition;
        }
    }

    private void OnDestroy()
    {
        // 오브젝트 파괴 시 버튼 이벤트 제거
        if (useButton != null) useButton.onClick.RemoveListener(UseSelectedItem);
        if (dropButton != null) dropButton.onClick.RemoveListener(DropSelectedItem);
        if (sellButton != null) sellButton.onClick.RemoveListener(SellSelectedItem);
    }
}