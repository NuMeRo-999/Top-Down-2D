using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>(20); // Lista con 20 espacios
    public int selectedIndex = 0; // Índice del objeto seleccionado
    public WeaponSystem weaponSystem; // Referencia al sistema de armas

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

    void Update()
    {
        HandleSelectionInput();
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
        else if (scroll < 0f)
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
        InventoryItem selectedItem = items[selectedIndex];

        if (selectedItem != null)
        {
            if (selectedItem.isWeapon)
            {
                weaponSystem.EquipWeapon(selectedItem.weaponStats);
            }
            else if (selectedItem.isHealingItem)
            {
                Debug.Log($"Objeto curativo seleccionado: {selectedItem.itemName}");
                // Podrías agregar más lógica para el uso del objeto curativo
            }
        }
        else
        {
            weaponSystem.UnequipWeapon();
            Debug.Log("Espacio vacío seleccionado.");
        }
    }
}
