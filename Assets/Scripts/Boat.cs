using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public float speed = 3;
    private SpriteRenderer sr;
    public GameObject player;
    private SpriteRenderer playerSR;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerSR = player.GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        // Move the boat across the screen
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

        // Keep the boat on screen
        if (transform.position.x > 20)
            transform.position += new Vector3(-40, 0, 0);

        if (transform.position.x < -20)
            transform.position += new Vector3(40, 0, 0);

        // Update the sorting order
        if (player.transform.position.y < transform.position.y)
            sr.sortingOrder = playerSR.sortingOrder - 1 - (int)transform.position.y; // Also keeps the boats from overlapping each other
        else
            sr.sortingOrder = playerSR.sortingOrder + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the play is on a boat we set the player parent to the boat so they stay on it
        if (collision.tag == "Player")
        {
            collision.transform.SetParent(transform);
            Debug.Log("On Boat");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the player leaves the boat we reset their parent to null
        if (collision.tag == "Player")
        {
            if (collision.transform.parent == transform)
            {
                collision.transform.SetParent(null);
            }

            Debug.Log("Off Boat");
        }
    }
}
