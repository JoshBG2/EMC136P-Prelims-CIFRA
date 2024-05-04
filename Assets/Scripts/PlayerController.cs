using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    /* --------------------------------------------------INSTANTIATING VARIABLES-------------------------------------------------- */

    public float speed = 5f;
    public float rotationSpeed = 100f;
    public bool hasKey = false;
    public bool levelComplete = false;

    public int lives = 3; 
    private Vector3 originalPosition;

    public LivesManager livesManager;

    void Start()
    {
        originalPosition = transform.position;
    }

    /* --------------------------------------------------PLAYER CONTROLS-------------------------------------------------- */

    // PLAYER WSAD MOVEMENT
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f), rotationSpeed * Time.deltaTime);
        }

        transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);
    }

    // COLLISIONS
    void OnCollisionEnter(Collision collision)
    {
        // MOB AND PLAYER COLLISION
        if (collision.gameObject.CompareTag("Mob"))
        {
            // Lose 1 life
            LoseLife(); 
            if (lives <= 0)
            {
                SceneManager.LoadScene("Game Over Scene"); 
            }
            else
            {
                ResetToOriginalPosition(); 
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // KEY COLLISION
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            Destroy(other.gameObject);
        }
        // END PLATFORM COLLISIONS
        else if (other.CompareTag("EndPlatform"))
        {
            levelComplete = true;
            if (hasKey)
            {
                // LOAD NEXT LEVEL
                if (SceneManager.GetActiveScene().buildIndex < 3)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    SceneManager.LoadScene("Win Scene"); // If Level 3 is Cleared
                }
            }
        }
    }

    // RESET PLAYER POSITION
    void ResetToOriginalPosition()
    {
        transform.position = originalPosition;
    }

    // PLAYER LOSES A LIFE
    void LoseLife()
    {
        lives--;
        livesManager.UpdateLivesText();
        Debug.Log("Remaining Lives: " + lives);
    }
}
