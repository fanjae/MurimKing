using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int count = 1;

    public ItemData ItemData => itemData;
    public int Count => count;

    public bool TryPickup(Inventory inventory)
    {
        if (inventory == null) return false;
        if (itemData == null) return false;
        if (count <= 0) return false;

        bool added = inventory.AddItem(itemData, count);
        Debug.Log($"Pickup AddItem result: {added}, item: {itemData.ItemName}, count: {count}");
        if (added)
        {
            Destroy(gameObject);
        }

        return added;
    }
}