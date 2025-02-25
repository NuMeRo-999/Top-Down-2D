using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class BossArea : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera; // Cambiado a CinemachineVirtualCamera
    [SerializeField] private float lensInsideArea = 10f;
    [SerializeField] private float lensOutsideArea = 5f;
    [SerializeField] private float transitionSpeed = 2f; // Velocidad de la transición

    private Coroutine currentTransition;
    public GameObject boss;
    public Enemy enemy;
    public GameObject winCanvas;

    private bool hasWon = false; // Variable para evitar múltiples ejecuciones de la animación

    private void Start()
    {
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        enemy = FindFirstObjectByType<Boss>().GetComponent<Enemy>();
        winCanvas.SetActive(false); // Asegúrate de que el canvas esté deshabilitado al inicio
    }

    private void Update()
    {
        CheckWin();
    }

    private void CheckWin()
    {
        if (!hasWon && enemy.currentHealth <= 0) // Verifica si ya se ha ganado
        {
            hasWon = true; // Marca que la victoria ha sido registrada
            StartCoroutine(FadeInCanvas(winCanvas, 1f)); // Inicia el fade in
        }
    }

    private IEnumerator FadeInCanvas(GameObject canvas, float duration)
    {
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = canvas.AddComponent<CanvasGroup>();
        }

        canvas.SetActive(true);
        canvasGroup.alpha = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f; // Asegura que el alpha sea 1 al final
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
