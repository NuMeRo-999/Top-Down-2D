using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    public Chest parentChest;
    private string triggerName;

    void Start()
    {
        triggerName = name;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        parentChest.OnChildTriggerEnter(other, triggerName);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        parentChest.OnChildTriggerExit(other, triggerName);
    }
}
