using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform targetPosition; // Posición a la que teletransportar
    [SerializeField] private float teleportDelay = 2f; // Tiempo antes de teletransportarse
    [SerializeField] private GameObject[] levels; // Array de niveles (GameObjects)
    [SerializeField] private int targetLevelIndex; // Índice del nivel al que se teletransporta
    private int currentLevelIndex; // Índice del nivel actual
    private bool isTeleporting; // Control para evitar múltiples teletransportes

    private void Start()
    {
        currentLevelIndex = 0; // Inicialmente se asume que estás en el nivel 0
        // UpdateLevelActivation();
    }

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
        if (targetPosition != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Mover al jugador a la posición objetivo
                player.transform.position = targetPosition.position;

                // Cambiar al nivel objetivo
                currentLevelIndex = targetLevelIndex;
                // UpdateLevelActivation();
            }
        }

        isTeleporting = false;
    }

    private void UpdateLevelActivation()
    {
        // Activar solo el nivel actual y desactivar los demás
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(i == currentLevelIndex);
        }
    }
}
