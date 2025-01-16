using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private string targetScene; // Nombre de la escena destino
    [SerializeField] private string targetSpawnPoint; // Nombre del spawn point en la escena destino
    [SerializeField] private float teleportDelay = 2f;
    private AudioSource audioSource;
    private Volume volume;

    private bool isTeleporting;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet<ChromaticAberration>(out var chromaticAberration))
        {
            chromaticAberration.active = false;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            isTeleporting = true;
            if (volume.profile.TryGet<ChromaticAberration>(out var chromaticAberration))
        {
            chromaticAberration.active = true;
        }
            audioSource.Play();

            Invoke(nameof(TeleportPlayer), teleportDelay);
        }
    }

    private void TeleportPlayer()
    {
        PlayerManager.Instance.SetSpawnPoint(targetSpawnPoint);
        SceneManager.LoadScene(targetScene);
    }
}
