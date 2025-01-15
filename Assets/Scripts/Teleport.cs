using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private string targetScene; // Nombre de la escena destino
    [SerializeField] private string targetSpawnPoint; // Nombre del spawn point en la escena destino
    [SerializeField] private float teleportDelay = 2f;

    private bool isTeleporting;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            isTeleporting = true;
            Invoke(nameof(TeleportPlayer), teleportDelay);
        }
    }

    private void TeleportPlayer()
    {
        PlayerManager.Instance.SetSpawnPoint(targetSpawnPoint);
        SceneManager.LoadScene(targetScene);
    }
}
