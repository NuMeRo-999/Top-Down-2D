using UnityEngine;

public class AnimationEvents : MonoBehaviour
{

    public GameObject explosionMarkPrefab;
    public GameObject wallExplosionMarkPrefab;


    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Explosion()
    {
        Instantiate(explosionMarkPrefab, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.identity);
        // Instantiate(explosionMarkPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, Random.Range(0, 360)));
    }

}
