using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject chestMark;
    public GameObject lootUI;
    public Inventory inventory;
    public List<InventoryItem> items = new List<InventoryItem>();
    public GameObject slotPrefab;
    public Sprite itemBackground;
    public bool openChest = false;

    private bool isPlayerInRange = false;
    public int selectedItemIndex = 0;

    void Start()
    {
        if (chestMark != null)
            chestMark.SetActive(false);

        if (lootUI != null)
            lootUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }

        if (lootUI.activeSelf)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
            {
                SelectItem(selectedItemIndex - 1);
            }
            else if (scroll < 0f)
            {
                SelectItem(selectedItemIndex + 1);
            }
        }
    }

    public void OnChildTriggerEnter(Collider2D other, string triggerName)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerName == "MarkTrigger")
            {
                chestMark.SetActive(true);
                Debug.Log("Mark trigger activated");
            }
            else if (triggerName == "InteractionTrigger")
            {
                isPlayerInRange = true;
                Debug.Log("Player in range to open chest");
            }
        }
    }

    public void OnChildTriggerExit(Collider2D other, string triggerName)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerName == "MarkTrigger")
            {
                chestMark.SetActive(false);
                Debug.Log("Mark trigger deactivated");
            }
            else if (triggerName == "InteractionTrigger")
            {
                isPlayerInRange = false;
                lootUI.SetActive(false);
                openChest = false;
                foreach (Transform child in lootUI.transform)
                {
                    Destroy(child.gameObject);
                }
                Debug.Log("Player out of range to open chest");
            }
        }
    }

    void OpenChest()
    {
        Debug.Log("Chest opened!");

        if (lootUI != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                InventoryItem item = items[i];
                GameObject slot = Instantiate(slotPrefab, lootUI.transform);
                float spacing = 10f;
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * (slot.GetComponent<RectTransform>().sizeDelta.y + spacing));
                
                slot.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
                slot.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

                // if (i == selectedItemIndex)
                // {
                //     slot.GetComponent<UnityEngine.UI.Image>().sprite = itemBackground;
                // }
            }
            lootUI.SetActive(true);
            openChest = true;
        }

        chestMark.SetActive(false);
        isPlayerInRange = false;   
    }

    void SelectItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return;
        }

        selectedItemIndex = index;

        foreach (Transform child in lootUI.transform)
        {
            var image = child.GetComponent<UnityEngine.UI.Image>();
            if (child.GetSiblingIndex() == selectedItemIndex)
            {
            image.enabled = true;
            }
            else
            {
            image.enabled = false;
            }
        }

        Transform selectedItem = lootUI.transform.GetChild(selectedItemIndex);
        var selectedItemImage = selectedItem.GetComponent<UnityEngine.UI.Image>();
        selectedItemImage.sprite = itemBackground;
        selectedItemImage.enabled = true;
    }
}
