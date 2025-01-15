using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject chestMark;
    public GameObject lootUI;
    public GameObject lootUICanvas;
    public Inventory inventory;
    public List<InventoryItem> items = new List<InventoryItem>();
    public GameObject slotPrefab;
    public Sprite itemBackground;
    private Animator animator;
    public bool openChest = false;

    private bool isPlayerInRange = false;
    public int selectedItemIndex = 0;

    public AudioSource pickObjectAudioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        pickObjectAudioSource = GetComponent<AudioSource>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<Inventory>();
        }

        if (chestMark != null)
            chestMark.SetActive(false);

        if (lootUI != null)
            lootUI.SetActive(false);

        if (lootUICanvas != null)
            lootUICanvas.SetActive(false);
    }

    void Update()
    {
        animator.SetBool("isOpen", openChest);

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

            if (Input.GetMouseButtonDown(0))
            {
                CollectItem();
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
                lootUICanvas.SetActive(false);
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
            foreach (Transform child in lootUI.transform)
            {
                Destroy(child.gameObject);
            }

            selectedItemIndex = 0;

            for (int i = 0; i < items.Count; i++)
            {
                InventoryItem item = items[i];
                GameObject slot = Instantiate(slotPrefab, lootUI.transform);
                float spacing = 10f;
                float initialHeight = 35f;
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, initialHeight - i * (slot.GetComponent<RectTransform>().sizeDelta.y + spacing));

                var text = slot.GetComponentInChildren<TextMeshProUGUI>();
                text.text = item.itemName;
                text.color = Color.white;

                var image = slot.GetComponent<UnityEngine.UI.Image>();
                if (i == selectedItemIndex)
                {
                    image.sprite = itemBackground;
                    image.enabled = true;
                }
                else
                {
                    image.enabled = false;
                }
            }

            lootUI.SetActive(true);
            lootUICanvas.SetActive(true);
            openChest = true;
        }

        chestMark.SetActive(false);
        isPlayerInRange = false;
    }

    void SelectItem(int index)
    {
        if (index < 0 || index >= items.Count) return;

        selectedItemIndex = index;

        for (int i = 0; i < lootUI.transform.childCount; i++)
        {
            var child = lootUI.transform.GetChild(i);
            var image = child.GetComponent<UnityEngine.UI.Image>();

            if (i == selectedItemIndex)
            {
                image.sprite = itemBackground;
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }
    }

    void CollectItem()
    {
        if (selectedItemIndex < 0 || selectedItemIndex >= items.Count) return;

        InventoryItem selectedItem = items[selectedItemIndex];

        Debug.Log(inventory);

        if (inventory.HasAvailableSpace())
        {
            inventory.AddItem(selectedItem);
            pickObjectAudioSource.Play();

            items.RemoveAt(selectedItemIndex);
            foreach (Transform child in lootUI.transform)
            {
                Destroy(child.gameObject);
            }

            OpenChest();

            if (items.Count == 0)
            {
                lootUI.SetActive(false);
                lootUICanvas.SetActive(false);
                openChest = false;
                Debug.Log("All items collected, chest is empty!");
            }
        }
        else
        {
            Debug.Log("No space in inventory to collect item.");
        }
    }

}
