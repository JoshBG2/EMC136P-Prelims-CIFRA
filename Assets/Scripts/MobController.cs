using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    /* --------------------------------------------------INSTANTIATING VARIABLES-------------------------------------------------- */

    // Mob Properties
    public List<Transform> waypoints = new List<Transform>();
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public LayerMask playerLayer;

    private int currentWaypointIndex = 0; 
    private Transform player;
    private Renderer childRenderer; 
    private Color originalColor;  
    
    //Raycast Properties
    public float raycastLength = 10f; 
    public int rayCount = 3; 
    public float coneAngle = 45f;

    /* --------------------------------------------------MOB BEHAVIOR-------------------------------------------------- */

    void Start()
    {
        // Obtaining Color Values of the Mob
        childRenderer = GetComponentInChildren<Renderer>();
        originalColor = childRenderer.material.color;
    }

    void Update()
    {
        // IF LIST IS NOT EMPTY
        if (waypoints.Count == 0) return;

        // DETECT PLAYER IF STATEMENT
        if (DetectPlayer())
        {
            childRenderer.material.color = Color.red;
            MoveTowardsPlayer();
        }
        else
        {
            childRenderer.material.color = originalColor;
            MoveTowardsWaypoint();
        }
    }

    // PLAYER RAYCAST DETECTION
    bool DetectPlayer()
    {
        float halfConeAngle = coneAngle / 2f;
        Quaternion startRotation = Quaternion.AngleAxis(-halfConeAngle, transform.up);
        Vector3 raycastDirection = transform.forward;

        for (int i = 0; i < rayCount; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(i * coneAngle / (rayCount - 1), transform.up);
            Vector3 direction = rotation * startRotation * raycastDirection;

            // RAYCAST IF STATEMENT
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, raycastLength, playerLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    player = hit.collider.transform;
                    Debug.DrawRay(transform.position, direction * hit.distance, Color.red); 
                    return true;
                }
            }
            Debug.DrawRay(transform.position, direction * raycastLength, Color.blue); 
        }
        return false;
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector3 targetDirection = player.position - transform.position;
        targetDirection.y = 0f;

        // Rotate towards Player
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move towards  Player
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    // MOB TO WAYPOINT
    void MoveTowardsWaypoint()
    {
        Vector3 targetDirection = waypoints[currentWaypointIndex].position - transform.position;
        targetDirection.y = 0f;

        // Rotate towards current waypoint
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move towards current waypoint
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // If mob reaches current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.5f)
        {
            // Move to the next waypoint
            currentWaypointIndex++;

            // Reset the waypoint index if exceeds the number of waypoints on the list
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
            }
        }
    }
}

