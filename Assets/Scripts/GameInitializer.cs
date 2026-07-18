using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory가 연결되지 않았습니다.");
            return;
        }

        PlayerInventoryController controller =
            playerInventory.Controller;

        if (controller == null)
        {
            Debug.LogError(
                "PlayerInventoryController가 아직 초기화되지 않았습니다."
            );

            return;
        }

        ShopNPC[] shops =
            FindObjectsByType<ShopNPC>(
                FindObjectsSortMode.None
            );

        foreach (ShopNPC shop in shops)
        {
            shop.Initialize(controller);
        }
    }
}