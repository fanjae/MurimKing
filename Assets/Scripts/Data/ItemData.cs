using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Etc
}

public abstract class ItemData : ScriptableObject
{
    public int Id;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    public ItemType type;
    public bool isStackable = true;
    public int maxStackCount = 99;
}