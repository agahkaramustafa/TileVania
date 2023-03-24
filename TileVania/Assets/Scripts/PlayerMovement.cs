using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRB;
    Animator animator;
    Collider2D playerCollider;

    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;

    float startingGravityScale;

    void Start() 
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        startingGravityScale = playerRB.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRB.velocity.y);
        playerRB.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRB.velocity.x) > Mathf.Epsilon;

        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRB.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRB.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) 
        {
            playerRB.gravityScale = startingGravityScale;
            return;
        }

        Vector2 climbVelocity = new Vector2(playerRB.velocity.x, moveInput.y * climbSpeed);
        playerRB.velocity = climbVelocity;
        playerRB.gravityScale = 0f;

        bool playerHasClimbingSpeed = Mathf.Abs(playerRB.velocity.y) > Mathf.Epsilon;

        animator.SetBool("isClimbing", playerHasClimbingSpeed);

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            playerRB.velocity += new Vector2(0f, jumpSpeed); 
        }   
    }
}
