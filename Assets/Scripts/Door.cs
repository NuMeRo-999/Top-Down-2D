using UnityEngine;

public class Door : MonoBehaviour
{

    public bool open;
    private Animator animator;
    public Collider2D collider;

    void Start()
    {
        animator = GetComponent<Animator>();
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
        animator.SetBool("Open", true);
        collider.enabled = false;
    }

    public void closeDoor()
    {
        open = false;
        animator.SetBool("Open", false);
        collider.enabled = true;
    }

}
