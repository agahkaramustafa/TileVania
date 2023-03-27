using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int coinScoreAmount = 100;

    bool wasCollected = false;

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (!wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(coinScoreAmount);
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
