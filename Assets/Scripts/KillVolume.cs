using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if ((!playerController.isMoving) && (collision.transform.parent == null)) // If the player is not moving and they're not on any boats
            {
                Debug.Log("Player Drowned"); // Player dies
                GameManager.instance.KillPlayer(); // Reset the player
            }
        }
    }
}
