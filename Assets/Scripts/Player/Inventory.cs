using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>(20); // Inventario con 20 espacios
    public int selectedIndex = 0;
    public WeaponSystem weaponSystem;
    private Player player;
    public InventoryItem selectedItem;
    public TextMeshProUGUI ammoText;

    public Chest[] chest;

    void Awake()
    {
        // Rellena espacios vacíos con null
        while (items.Count < 20)
            items.Add(null);

        Debug.Log($"El inventario tiene {items.Count} espacios.");

        UpdateEquippedItem();
    }


    private void Start()
    {
        player = GetComponent<Player>();
        chest = FindObjectsByType<Chest>(FindObjectsSortMode.None);
    }

    void Update()
    {
        HandleSelectionInput();

        // Usa el objeto seleccionado al hacer clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            selectedItem = items[selectedIndex];

            if (selectedItem != null && selectedItem.isHealingItem)
            {
                player.Heal(selectedItem.healingAmount);
                items[selectedIndex] = null; // Elimina el objeto usado del inventario
            }
        }
    }

    void HandleSelectionInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");


        foreach (Chest chest in chest)
        {
            if (chest.openChest) return;
        }
        

        if (scroll > 0f) // Cambia al espacio anterior en el inventario
        {
            do
            {
                selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : items.Count - 1;
            } while (items[selectedIndex] == null && !IsInventoryEmpty());
            UpdateEquippedItem();
        }
        else if (scroll < 0f || Input.GetKeyDown(KeyCode.DownArrow)) // Cambia al siguiente espacio
        {
            do
            {
                selectedIndex = (selectedIndex < items.Count - 1) ? selectedIndex + 1 : 0;
            } while (items[selectedIndex] == null && !IsInventoryEmpty());
            UpdateEquippedItem();
        }
    }

    void UpdateEquippedItem()
    {
        selectedItem = items[selectedIndex];

        if (selectedItem != null && selectedItem.isWeapon)
        {
            if (selectedItem.weaponStats is Shotgun shotgun)
            {
                weaponSystem.EquipWeapon(shotgun);
                Debug.Log("Arma equipada correctamente como Shotgun.");
            }
            else
            {
                weaponSystem.EquipWeapon(selectedItem.weaponStats);
                Debug.Log($"Arma equipada: {selectedItem.weaponStats.GetType().Name}");
            }
        }
        else
        {
            weaponSystem.UnequipWeapon();
            Debug.Log("Espacio vacío seleccionado.");
        }
    }

    public void AddItem(InventoryItem item)
    {
        if (item.isWeapon)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && items[i].weaponStats != null && items[i].weaponStats.weaponName == item.weaponStats.weaponName)
                {
                    items[i].weaponStats.totalAmmo += 10;
                    ammoText.text = "ammo =" + item.weaponStats.currentAmmo.ToString() + "/" + item.weaponStats.totalAmmo.ToString();
                    return;
                }
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                break;
            }
        }
        UpdateEquippedItem();
    }

    bool IsInventoryEmpty()
    {
        foreach (var item in items)
        {
            if (item != null)
                return false;
        }
        return true;
    }
}
