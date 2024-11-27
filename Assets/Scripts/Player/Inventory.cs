using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>(20);
    public int selectedIndex = 0;
    public WeaponSystem weaponSystem;
    private Player player;
    public InventoryItem selectedItem;
    public TextMeshProUGUI ammoText;

    void Awake()
    {
        if (items.Count < 20)
        {
            // Rellenar espacios vacíos con null
            for (int i = items.Count; i < 20; i++)
                items.Add(null);
        }
        Debug.Log(items.Count);

        UpdateEquippedItem();
    }

    private void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        HandleSelectionInput();

        if (Input.GetMouseButtonDown(0))
        {
            selectedItem = items[selectedIndex];

            if (selectedItem != null)
            {
                if (selectedItem.isHealingItem)
                {
                    player.Heal(selectedItem.healingAmount);
                    items[selectedIndex] = null;
                }
            }
        }
    }

    void HandleSelectionInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            do
            {
                selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : items.Count - 1;
            } while (items[selectedIndex] == null);
            UpdateEquippedItem();
        }
        else if (scroll < 0f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            do
            {
                selectedIndex = (selectedIndex < items.Count - 1) ? selectedIndex + 1 : 0;
            } while (items[selectedIndex] == null);
            UpdateEquippedItem();
        }
    }

    void UpdateEquippedItem()
    {
        selectedItem = items[selectedIndex];
        if (!selectedItem.isWeapon) ammoText.enabled = false;
        else ammoText.enabled = true;

        if (selectedItem != null)
        {
            if (selectedItem.isWeapon)
            {
                weaponSystem.EquipWeapon(selectedItem.weaponStats);
            }
        }
        else
        {
            weaponSystem.UnequipWeapon();
            Debug.Log("Espacio vacío seleccionado.");
        }
    }
}
