using System.Collections.Generic;
using System.Linq;
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
        chest = FindObjectsByType<Chest>(FindObjectsSortMode.None);
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

        // Verificar si hay cofres abiertos y salir si es el caso
        foreach (Chest chest in chest)
        {
            if (chest.openChest) return;
        }

        // Desplazarse hacia arriba en el inventario
        if (scroll > 0f || Input.GetKeyDown(KeyCode.UpArrow))
        {
            int initialIndex = selectedIndex; // Guardar índice inicial para evitar bucles infinitos
            do
            {
                selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : items.Count - 1;
            } while ((items[selectedIndex] == null || string.IsNullOrEmpty(items[selectedIndex].itemName)) && selectedIndex != initialIndex);

            UpdateEquippedItem();
        }
        // Desplazarse hacia abajo en el inventario
        else if (scroll < 0f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            int initialIndex = selectedIndex; // Guardar índice inicial para evitar bucles infinitos
            do
            {
                selectedIndex = (selectedIndex < items.Count - 1) ? selectedIndex + 1 : 0;
            } while ((items[selectedIndex] == null || string.IsNullOrEmpty(items[selectedIndex].itemName)) && selectedIndex != initialIndex);

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
            }
            else
            {
                weaponSystem.EquipWeapon(selectedItem.weaponStats);
            }
        }
        else
        {
            weaponSystem.UnequipWeapon();
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
            if (items[i] == null || string.IsNullOrEmpty(items[i].itemName))
            {
                items[i] = item;
                break;
            }
        }
        UpdateEquippedItem();
    }

    public bool HasAvailableSpace()
    {
        Debug.Log("Verificando espacios disponibles en el inventario...");
        foreach (var item in items)
        {
            // Verificar si el objeto es null antes de acceder a itemName
            if (item == null || string.IsNullOrEmpty(item.itemName))
            {
                Debug.Log("Espacio disponible encontrado.");
                return true;
            }
        }
        Debug.Log("No hay espacios disponibles.");
        return false;
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
