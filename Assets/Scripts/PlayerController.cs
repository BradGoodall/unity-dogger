using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float slideSpeed = 5; // Time taken to slide between locations
    private float slideValue = 0;
    private Vector2 slideVector = Vector2.zero;
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer sr;

    private Vector3 targetPosition = Vector3.zero;
    private Vector3 currentPosition;
    public bool isMoving = false;



    private void Start()
    {
        currentPosition = transform.position;
        animator = gameObject.GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        ManageInputs();

        animator.SetBool("Moving", isMoving);
        animator.SetFloat("Horizontal", slideVector.x);
        animator.SetFloat("Vertical", slideVector.y);

        ProcessSlide();

        sr.sortingOrder = (int)-transform.position.y;
    }

    private void ManageInputs()
    {
        if (!isMoving && !GameManager.instance.isGameOver())
        {
            // If the player uses WASD or Arrow Keys
            if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                slideVector = new Vector2(0, 1);
                InvokeSlide();
            }
            if ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.DownArrow)))
            {
                slideVector = new Vector2(0, -1);
                InvokeSlide();
            }
            if (((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow))) && (transform.parent == null)) // We can only move left/right if we're not on a boat
            {
                slideVector = new Vector2(-1, 0);
                InvokeSlide();
            }
            if (((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.RightArrow))) && (transform.parent == null))
            {
                slideVector = new Vector3(1, 0);
                InvokeSlide();
            }
        }
    }

    private void InvokeSlide()
    {
        // Set up the slide
        slideValue = 0;
        // Set the current position to slide from
        currentPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        // Set the position to slide to
        targetPosition = new Vector3(slideVector.x, slideVector.y, 0);
        // Play the movement sound
        audioSource.Play();
    }

    private void ProcessSlide()
    {
        // Perform the slide
        slideValue = Mathf.MoveTowards(slideValue, 1, slideSpeed * Time.deltaTime);

        // If we're moving, lerp to the target position
        if (isMoving)
            transform.position = Vector3.Lerp(currentPosition, currentPosition + targetPosition, slideValue);

        if (slideValue == 1)
            isMoving = false;
        else
            isMoving = true;
    }
}
