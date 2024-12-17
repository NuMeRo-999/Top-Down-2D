using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject chestMark;
    public GameObject lootUI;   

    private bool isPlayerInRange = false;

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
                Debug.Log("Player out of range to open chest");
            }
        }
    }

    void OpenChest()
    {
        Debug.Log("Chest opened!");

        if (lootUI != null)
        {
            lootUI.SetActive(true);
        }

        chestMark.SetActive(false);
        isPlayerInRange = false;   
    }
}
