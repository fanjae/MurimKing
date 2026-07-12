using TMPro;
using UnityEngine;

public class ItemTooltipUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text priceText;

    [Header("위치 설정")]
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private float spacing = 20f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.pivot = new Vector2(1f, 0.5f); 

        // 게임 시작시 패널 비활성화
        gameObject.SetActive(false);
    }

    public void Show(ItemData itemData)
    {
        // 표시할 아이템이 없으면 아무 작업도 하지 않음
        if (itemData == null) return;

        // ItemData 정보 UI 텍스트에 반영
        nameText.text = itemData.ItemName;
        descriptionText.text = itemData.Description;
        priceText.text = $"{itemData.Price}G";

        // 툴팁 활성화 뒤 위치 갱신
        gameObject.SetActive(true);
        UpdatePosition();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdatePosition()
    {
        // 위치 기준이 되는 인벤토리 패널이 연결되어 있는지 검사
        if (inventoryPanel == null)
        {
            Debug.LogError("ItemTooltipUI의 Inventory Panel이 연결되지 않았습니다.",this);
            return;
        }

        // 툴팁의 배치를 인벤토리 패널의 왼쪽 중앙으로 배치하도록 의도.
        Vector3 inventoryLeftCenter = inventoryPanel.TransformPoint(inventoryPanel.rect.xMin,inventoryPanel.rect.center.y,0f);

        rectTransform.position = inventoryLeftCenter + Vector3.left * spacing;
    }
}