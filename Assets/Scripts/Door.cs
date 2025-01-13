using UnityEngine;

public class Door : MonoBehaviour
{

    public bool open;
    private Animator animator;
    [SerializeField] private Collider2D doorCollider;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        animator.SetBool("Open", open);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Bullet")
            openDoor();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Bullet")
            closeDoor();
    }

    public void openDoor()
    {
        open = true;
        doorCollider.enabled = false;
        audioSource.Play();
    }

    public void closeDoor()
    {
        open = false;
        doorCollider.enabled = true;
        audioSource.Play();
    }

}
