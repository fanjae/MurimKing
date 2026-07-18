using UnityEngine;

public class ShopNPCInteraction : MonoBehaviour
{
    [SerializeField] private ShopNPC shopNPC;

    // 플레이어의 상호작용 입력을 확인할 입력 관리자
    private InputManager inputManager;

    // 플레이어가 상점 NPC의 상호작용 범위 안에 있는지 여부
    private bool isPlayerInRange;

    // 현재 상점 UI가 열려 있는지 여부
    private bool isShopOpen;

    private void Awake()
    {
        // 현재 씬에서 InputManager를 찾아 참조
        inputManager = FindAnyObjectByType<InputManager>();

        if (inputManager == null)
        {
            Debug.LogWarning("InputManager를 찾을 수 없습니다.", this);
        }

        if (shopNPC == null)
        {
            Debug.LogWarning("ShopNPC가 연결되지 않았습니다.", this);
        }
    }

    private void Update()
    {
        // 플레이어가 상호작용 범위 밖에 있으면 입력 처리하지 않음
        if (!isPlayerInRange) return;

        // InputManager가 없으면 입력 처리 불가능
        if (inputManager == null) return;

        // 상호작용 키가 입력되면 상점 열기 또는 닫기
        if (inputManager.InteractPressed)
        {
            ToggleShop();
        }
    }

    // 현재 상점 상태에 따라 열기 또는 닫기
    private void ToggleShop()
    {
        if (shopNPC == null) return;

        // 상점이 열려 있으면 닫기
        if (isShopOpen)
        {
            shopNPC.CloseShop();
            isShopOpen = false;
            return;
        }

        // 상점이 닫혀 있으면 열기
        shopNPC.OpenShop();
        isShopOpen = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player 태그를 가진 오브젝트만 상호작용 대상으로 처리
        if (!other.CompareTag("Player")) return;

        // 플레이어가 상점 상호작용 범위에 진입
        isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Player 태그를 가진 오브젝트만 처리
        if (!other.CompareTag("Player")) return;

        // 플레이어가 상점 상호작용 범위를 벗어남
        isPlayerInRange = false;

        // 범위를 벗어날 때 상점이 열려 있다면 자동으로 닫기
        if (isShopOpen)
        {
            shopNPC.CloseShop();
            isShopOpen = false;
        }
    }

    private void OnDisable()
    {
        // 오브젝트 비활성화 중 상점이 열려 있으면 상태 정리
        if (isShopOpen && shopNPC != null)
        {
            shopNPC.CloseShop();
        }

        isShopOpen = false;
        isPlayerInRange = false;
    }
}