public class PlayerInventoryController
{
    private readonly Inventory inventory;
    private readonly ItemDatabase itemDatabase;

    public PlayerInventoryController(Inventory inventory, ItemDatabase itemDatabase)
    {
        this.inventory = inventory;
        this.itemDatabase = itemDatabase;
    }

    public bool UseConsumableFromInventory(int slotIndex, Player player)
    {
        if (player == null) return false;

        if (!inventory.TryGetSlot(slotIndex, out InventorySlot inventorySlot))
            return false;

        if (inventorySlot.IsEmpty) return false;

        ItemData itemData = itemDatabase.GetItem(inventorySlot.ItemId);
        if (itemData == null) return false;

        if (itemData is not ConsumableData consumableData)
            return false;

        bool used = consumableData.Use(player);
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
}