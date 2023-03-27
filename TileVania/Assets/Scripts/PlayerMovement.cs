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
    CapsuleCollider2D playerCollider;
    BoxCollider2D feetCollider;
    Vector2 deathKick = new Vector2(0f, 50f);

    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    float startingGravityScale;
    bool isAlive = true;

    void Start() 
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        startingGravityScale = playerRB.gravityScale;
    }

    void Update()
    {
        if (!isAlive) {return;}

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        Shoot();
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
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) 
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

    void Die()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            playerRB.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        } 
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) {return;}

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) {return;}

        if (value.isPressed && feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            playerRB.velocity += new Vector2(0f, jumpSpeed); 
        }   
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }
}
