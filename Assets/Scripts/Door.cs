using UnityEngine;

public class Door : MonoBehaviour
{

    public bool open;
    private Animator animator;
    [SerializeField] private Collider2D doorCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Open", open);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        openDoor();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        closeDoor();
    }

    public void openDoor()
    {
        open = true;
        doorCollider.enabled = false;
    }

    public void closeDoor()
    {
        open = false;
        doorCollider.enabled = true;
    }

}
