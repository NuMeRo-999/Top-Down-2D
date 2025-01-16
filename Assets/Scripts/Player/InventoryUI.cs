using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform uiContainer; // Contenedor de la UI (con 20 botones/imágenes)
    public GameObject slotPrefab; // Prefab de un espacio en el inventario
    public Sprite itemBackground;

    private List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        // Crear los 20 slots
        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject slot = Instantiate(slotPrefab, uiContainer);
            float spacing = 10f; // Espacio entre los slots
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * (slot.GetComponent<RectTransform>().sizeDelta.y + spacing));
            slots.Add(slot);
        }

        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            InventoryItem item = inventory.items[i];
            Image icon = slots[i].GetComponent<Image>();
            TextMeshProUGUI name = slots[i].GetComponentInChildren<TextMeshProUGUI>();
            icon.enabled = item != null;

            if (item != null)
            {
                icon.sprite = itemBackground;
                name.text = string.IsNullOrEmpty(item.itemName) ? "-" : item.itemName;
                name.color = Color.white;
            }

            // slots[i].GetComponent<Image>().color = (i == inventory.selectedIndex) ? Color.green : Color.white; // Resaltar el seleccionado
            if (i == inventory.selectedIndex)
            {
                // Debug.Log("Seleccionado: " + item.itemName);
                icon.enabled = true;
                icon.sprite = itemBackground;
                // name.color = Color.black;
            }
            else
            {
                icon.enabled = false;
                icon.sprite = null;
            }
        }
    }
}
