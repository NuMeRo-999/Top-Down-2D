using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float orbitFrequency = 2f;
    [SerializeField] private float orbitMagnitude = 1.5f;
    [SerializeField] private float changeDirectionInterval = 1.5f;

    [Header("Step Sound Settings")]
    [SerializeField] private AudioSource stepAudioSource;  // AudioSource para los sonidos de los pasos
    [SerializeField] private AudioClip[] stepSounds;  // Array de sonidos de pasos
    [SerializeField] private float stepInterval = 0.5f; // Intervalo mínimo entre pasos

    private Animator animator;
    private Vector2 movementDirection;
    private bool isOrbitingPlayer = false;
    private float timeCounter;
    private float changeDirectionTimer;
    private Vector2 orbitOffset;
    private float stepTimer;  // Temporizador para controlar el intervalo entre pasos

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        changeDirectionTimer = changeDirectionInterval;
        stepTimer = stepInterval;  // Inicializar el temporizador de los pasos
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Dirección hacia el jugador
        Vector2 directionToPlayer = ((Vector2)player.position - rb.position).normalized;

        if (isOrbitingPlayer)
        {
            // Temporizador para cambiar la dirección del orbitado
            changeDirectionTimer -= Time.fixedDeltaTime;
            if (changeDirectionTimer <= 0f)
            {
                // Cambiar la dirección del orbitado aleatoriamente
                orbitOffset = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)).normalized * orbitMagnitude;
                changeDirectionTimer = changeDirectionInterval;
            }

            // Movimiento de órbita alrededor del jugador
            timeCounter += Time.fixedDeltaTime * orbitFrequency;
            Vector2 orbitMovement = new Vector2(
                Mathf.Sin(timeCounter) * orbitOffset.x,
                Mathf.Cos(timeCounter) * orbitOffset.y);

            // Dirección combinada (hacia el jugador + movimiento de órbita)
            movementDirection = (directionToPlayer + orbitMovement).normalized;
        }
        else
        {
            // Movimiento directo hacia el jugador
            movementDirection = directionToPlayer;
        }

        // Aplicar la velocidad usando linearVelocity
        rb.linearVelocity = movementDirection * speed;

        // Hacer que el enemigo mire hacia el jugador
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        // Reproducir sonido de pasos si el enemigo se está moviendo
        HandleStepSounds();
    }

    private void HandleStepSounds()
    {
        if (rb.linearVelocity.magnitude > 0.1f) // Si el enemigo está moviéndose
        {
            stepTimer -= Time.fixedDeltaTime;

            if (stepTimer <= 0f)
            {
                // Reproducir un sonido aleatorio de pasos
                if (stepSounds.Length > 0)
                {
                    AudioClip randomStepSound = stepSounds[Random.Range(0, stepSounds.Length)];
                    stepAudioSource.PlayOneShot(randomStepSound);
                }

                // Reiniciar el temporizador
                stepTimer = stepInterval;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isOrbitingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isOrbitingPlayer = false;
        }
    }
}
