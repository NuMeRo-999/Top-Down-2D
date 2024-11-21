using UnityEngine;

public class Door : MonoBehaviour
{

    public bool open;
    private Animator animator;
    private Collider2D doorCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        doorCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            openDoor();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            closeDoor();
        }
    }

    public void openDoor()
    {
        open = true;
        GetComponent<Collider2D>().enabled = false;
        doorCollider.enabled = false;
    }

    public void closeDoor()
    {
        open = false;
        GetComponent<Collider2D>().enabled = true;
        doorCollider.enabled = true;
    }

}
