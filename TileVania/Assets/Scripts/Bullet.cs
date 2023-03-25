using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRB;
    PlayerMovement playerMovement;
    float bulletSpeed = 20f;
    float xSpeed;

    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        xSpeed = playerMovement.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        bulletRB.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Enemies"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);
    }
}
