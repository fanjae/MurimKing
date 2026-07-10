using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    private Inventory inventory;

    public void Init(Inventory inventory)
    {
        this.inventory = inventory;
    }

    private void OnTriggerEnter(Collider other)
    {
        WorldItem worldItem = other.GetComponent<WorldItem>();
        if (worldItem == null) return;

        worldItem.TryPickup(inventory);
    }
}