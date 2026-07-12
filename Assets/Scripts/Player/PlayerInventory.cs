using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private PlayerItemPickup itemPickup;

    private Inventory inventory;
    private PlayerInventoryController controller;

    public Inventory Inventory => inventory;
    public PlayerInventoryController Controller => controller;

    private void Awake()
    {
        itemDatabase.Initialize();

        inventory = new Inventory(12);
        controller = new PlayerInventoryController(inventory, itemDatabase);

        inventoryUI.Init(inventory);
        itemPickup.Init(inventory);
    }
}