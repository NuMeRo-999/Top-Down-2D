using UnityEngine;
using Unity.Cinemachine;

public class BossArea : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float lensInsideArea = 10f; // Tamaño de lente dentro del área
    [SerializeField] private float lensOutsideArea = 4f; // Tamaño de lente fuera del área

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cambia el tamaño de la lente al entrar en el área
            if (cinemachineCamera != null)
            {
                cinemachineCamera.Lens.OrthographicSize = lensInsideArea;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cambia el tamaño de la lente al salir del área
            if (cinemachineCamera != null)
            {
                cinemachineCamera.Lens.OrthographicSize = lensOutsideArea;
            }
        }
    }
}
