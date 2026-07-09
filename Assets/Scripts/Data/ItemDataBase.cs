using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemData> items;

    private Dictionary<int, ItemData> itemMap;

    private void OnEnable()
    {
        itemMap = new Dictionary<int, ItemData>();

        foreach (ItemData item in items)
        {
            if (item == null) continue;
            itemMap[item.Id] = item;
        }
    }

    public bool TryGet(int itemId, out ItemData itemData)
    {
        return itemMap.TryGetValue(itemId, out itemData);
    }

    public ItemData Get(int itemId)
    {
        if (!TryGet(itemId, out ItemData itemData))
        {
            Debug.LogError($"존재하지 않는 아이템 ID: {itemId}");
            return null;
        }

        return itemData;
    }
}