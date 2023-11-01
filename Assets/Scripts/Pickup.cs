using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private float timer; // Timer for the pickup
    private SpriteRenderer sr;

    private void Start()
    {
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        timer = Random.Range(8, 15); // Set the timer for the pickup
    }

    private void Update()
    {
        // Countdown the timer
        timer -= Time.deltaTime;

        // If the timer runs out, delete the pickup
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

        // If the times has less than a second left turn the pickup red
        if (timer<1)
            sr.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player hits the pickup, add score and delete
        if (collision.transform.tag == "Player")
        {
            GameManager.instance.AddScore(50);
            GameManager.instance.PlaySound(2);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Remove the pickup from the pickups list
        GameManager.instance.RemovePickup(this.gameObject);
    }
}
