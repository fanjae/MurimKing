public class PlayerInventoryController
{
    private readonly Inventory inventory;
    private readonly ItemDatabase itemDatabase;

    public PlayerInventoryController(Inventory inventory, ItemDatabase itemDatabase)
    {
        this.inventory = inventory;
        this.itemDatabase = itemDatabase;
    }

    public bool UseConsumableFromInventory(int slotIndex,PlayerHealth playerHealth)
    {
        if (playerHealth == null) return false;

        if (!inventory.TryGetSlot(slotIndex, out InventorySlot inventorySlot))
            return false;

        if (inventorySlot.IsEmpty) return false;

        ItemData itemData = itemDatabase.GetItem(inventorySlot.ItemId);
        if (itemData == null) return false;

        if (itemData is not ConsumableData consumableData)
            return false;

        bool used = consumableData.Use(playerHealth);
        if (!used) return false;

        return inventory.RemoveItemAt(slotIndex, 1);
    }

    public bool CanRemoveFromInventory(int slotIndex, int count = 1)
    {
        if (count <= 0) return false;
        if (!inventory.TryGetSlot(slotIndex, out InventorySlot inventorySlot)) return false;
        if (inventorySlot.IsEmpty) return false;

        return inventorySlot.Count >= count;
    }

    public bool RemoveFromInventory(int slotIndex, int count = 1)
    {
        if (!CanRemoveFromInventory(slotIndex, count)) return false;

        return inventory.RemoveItemAt(slotIndex, count);
    }

    public bool SwapInventorySlots(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex) return false;

        return inventory.SwapSlots(fromIndex, toIndex);
    }
    public bool CanAddToInventory(ItemData itemData, int count = 1)
    {
        if (itemData == null || count <= 0) return false;

        return inventory.CanAddItem(itemData, count);
    }

    public bool AddToInventory(ItemData itemData, int count = 1)
    {
        if (!CanAddToInventory(itemData, count)) return false;

        return inventory.AddItem(itemData, count);
    }
}