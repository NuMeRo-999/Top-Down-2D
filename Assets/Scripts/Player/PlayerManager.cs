using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private string spawnPointName; // Nombre del spawn point actual
    public Transform playerTransform { get; private set; } // Referencia al transform del jugador
    private GameObject playerInstance; // Referencia al jugador

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Buscar al jugador en la escena actual
        playerInstance = GameObject.FindGameObjectWithTag("Player");
        if (playerInstance != null)
        {
            playerTransform = playerInstance.transform;
            DontDestroyOnLoad(playerInstance); // Evitar que el jugador se destruya al cambiar de escena
        }
        else
        {
            Debug.LogError("No se encontró un objeto con la etiqueta 'Player' en la escena inicial.");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MovePlayerToSpawn();
    }

    public void MovePlayerToSpawn()
    {
        // Buscar el spawn point por su nombre en la escena actual
        GameObject spawnPoint = GameObject.Find(spawnPointName);
        spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint != null && playerTransform != null)
        {
            playerTransform.position = spawnPoint.transform.position; // Mover al jugador al spawn point
        }
        else
        {
            Debug.LogWarning($"Spawn point '{spawnPointName}' o jugador no encontrado en la escena {SceneManager.GetActiveScene().name}");
        }
    }

    public void SetSpawnPoint(string newSpawnPointName)
    {
        spawnPointName = newSpawnPointName;
    }
}
