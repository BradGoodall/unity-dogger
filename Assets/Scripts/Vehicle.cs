using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
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
        // Move the vehicle across the screen
        transform.position += new Vector3(-speed*Time.deltaTime, 0, 0);

        // Keep the vehicle on screen
        if (transform.position.x > 20)
            transform.position += new Vector3(-40, 0, 0);

        if (transform.position.x < -20)
            transform.position += new Vector3(40, 0, 0);

        // Update the sorting order
        if (player.transform.position.y < transform.position.y)
            sr.sortingOrder = playerSR.sortingOrder - 1;
        else
            sr.sortingOrder = playerSR.sortingOrder + 1;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the play is hit by a vehicle they die
        if (collision.tag == "Player" && collision.GetComponent<PlayerController>().isMoving == false)
        {
            Debug.Log("Player hit by Vehicle!");
            GameManager.instance.KillPlayer();
        }
    }
}
