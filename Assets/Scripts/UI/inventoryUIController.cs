using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private MonoBehaviour inputSource;

    private IPlayerInput input;

    private void Awake()
    {
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (input == null || !input.InventoryPressed)  return;

        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}