using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    [SerializeField] float speed = 4f;
    [SerializeField] float orbitFrequency = 2f;
    [SerializeField] float orbitMagnitude = 1.5f;
    [SerializeField] float changeDirectionInterval = 1.5f;

    private Vector2 movementDirection;
    private bool isOrbitingPlayer = false;
    private float timeCounter;
    private float changeDirectionTimer;
    private Vector2 orbitOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        changeDirectionTimer = changeDirectionInterval;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Dirección hacia el jugador
        Vector2 directionToPlayer = ((Vector2)player.position - rb.position).normalized;

        // Movimiento directo hacia el jugador
        Vector2 movementDirection = directionToPlayer;

        // Aplicar la velocidad usando linearVelocity
        rb.linearVelocity = movementDirection * speed;

        // Hacer que el enemigo mire hacia el jugador
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Activar el movimiento de órbita alrededor del jugador
            isOrbitingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Desactivar el movimiento de órbita
            isOrbitingPlayer = false;
        }
    }
}
