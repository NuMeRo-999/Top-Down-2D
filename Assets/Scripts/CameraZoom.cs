using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera; // Cambiado a CinemachineVirtualCamera
    [SerializeField] private float lensInsideArea = 10f;
    [SerializeField] private float lensOutsideArea = 5f;
    [SerializeField] private float transitionSpeed = 2f; // Velocidad de la transición

    private Coroutine currentTransition;

    private void Start()
    {
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cambia suavemente el tamaño de la lente al entrar en el área
            if (cinemachineCamera != null)
            {
                StartSmoothTransition(lensInsideArea);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cambia suavemente el tamaño de la lente al salir del área
            if (cinemachineCamera != null)
            {
                StartSmoothTransition(lensOutsideArea);
            }
        }
    }

    private void StartSmoothTransition(float targetSize)
    {
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition); // Detener la transición actual si está en curso
        }
        currentTransition = StartCoroutine(SmoothTransition(targetSize));
    }

    private IEnumerator SmoothTransition(float targetSize)
    {
        float startSize = cinemachineCamera.Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (Mathf.Abs(cinemachineCamera.Lens.OrthographicSize - targetSize) > 0.01f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime);
            yield return null;
        }

        cinemachineCamera.Lens.OrthographicSize = targetSize; // Asegurarse de que llegue al valor final
    }
}
