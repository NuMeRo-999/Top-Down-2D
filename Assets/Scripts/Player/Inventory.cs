using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>(20); // Lista con 20 espacios
    public int selectedIndex = 0; // Índice del objeto seleccionado
    public WeaponSystem weaponSystem; // Referencia al sistema de armas

    void Start()
    {
        if (items.Count < 20)
        {
            // Rellenar espacios vacíos con null
            for (int i = items.Count; i < 20; i++)
                items.Add(null);
        }

        UpdateEquippedItem();
    }

    void Update()
    {
        HandleSelectionInput();
    }

    void HandleSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // Cambiar al objeto anterior
        {
            selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : items.Count - 1;
            UpdateEquippedItem();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) // Cambiar al siguiente objeto
        {
            selectedIndex = (selectedIndex < items.Count - 1) ? selectedIndex + 1 : 0;
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
