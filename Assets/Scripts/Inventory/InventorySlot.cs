using System;

[Serializable]
public class InventorySlot
{
    public int ItemId { get; private set; }
    public int Count { get; private set; }

    public bool IsEmpty => ItemId <= 0 || Count <= 0;

    public void SetItem(ItemData itemData, int count)
    {
        if (itemData == null) throw new ArgumentNullException(nameof(itemData));
        if (count <= 0) throw new ArgumentException("Item count >= 1");
        if (itemData.ItemId <= 0) throw new ArgumentException("ItemId must be greater than 0");

        ItemId = itemData.ItemId;
        Count = count;
    }

    public void AddCount(int amount)
    {
        if (amount <= 0) return;

        Count += amount;
    }

    public void RemoveCount(int amount)
    {
        if (amount <= 0) return;

        Count -= amount;

        if (Count <= 0) Clear();
    }

    public void Clear()
    {
        ItemId = 0;
        Count = 0;
    }
}