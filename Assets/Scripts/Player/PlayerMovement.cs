using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  private Rigidbody2D rb;
  private Vector2 movementInput;
  private Vector2 smoothedMovementInput;
  private Vector2 movementInputSmoothVelocity;
  private Vector2 MousePos;
  private Camera cam;
  private Animator animator;
  public float speed = 5;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    cam = Camera.main;
    animator = GetComponent<Animator>();
  }

  void Update()
  {
    MousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
  }

  void FixedUpdate()
  {
    smoothedMovementInput = Vector2.SmoothDamp(
        smoothedMovementInput,
        movementInput,
        ref movementInputSmoothVelocity,
        0.1f);

    rb.linearVelocity = smoothedMovementInput * speed;
    Vector2 lookDir = MousePos - rb.position;

    float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    rb.rotation = angle;
  }

  private void OnMove(InputValue inputValue)
  {
    movementInput = inputValue.Get<Vector2>();
    if (movementInput != Vector2.zero)
      animator.SetBool("run", true);
    else
      animator.SetBool("run", false);

  }
}
