using UnityEngine;

public class Door : MonoBehaviour
{

    public bool open;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            
        }
    }
}
