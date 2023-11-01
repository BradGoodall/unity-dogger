using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        // By default the goals are not visible until they've been triggered by a player landing on them
        rb.Sleep();
        sr.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the player hits a goal we set the goal to active and add some points
        // When the goal is active the player cannot enter it again
        if (collision.tag == "Player" && !sr.enabled)
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if ((!playerController.isMoving) && (collision.transform.parent == null)) // If the player is not moving and they're not on any boats
            {
                Debug.Log("Goal Reached");
                GameManager.instance.AddScore(150);
                GameManager.instance.ResetPlayerPosition();
                GameManager.instance.AddGoal();

                rb.WakeUp();
                sr.enabled = true;
                bc.isTrigger = false;
            }
        }
    }
}
