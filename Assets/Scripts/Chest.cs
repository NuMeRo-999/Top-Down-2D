using UnityEngine;

public class Chest : MonoBehaviour
{

    public GameObject chestMark;
    void Start()
    {
        Debug.Log("Chest");
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Chest");
        if (other.tag == "Player")
        {
            chestMark.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
        {
            chestMark.SetActive(false);
        }
    }
}
