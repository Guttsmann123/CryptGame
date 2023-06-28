using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAndFollow : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 200.0f;
    public float range = 10.0f;
    public float followRange = 5.0f;
    public Transform player;

    private Vector3 startPosition;
    private Quaternion startRotation; // Save the initial rotation
    private bool movingTowardsEnd = true; // Keep track if we are moving towards the end point or the start point
    private Vector3 patrolDirection;
    private Vector3 endPosition; // The position we move to before turning around
    private bool isFollowingPlayer;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation; // Save the initial rotation
        patrolDirection = transform.forward; // Save the direction we start moving in
        endPosition = startPosition + patrolDirection * range;
        isFollowingPlayer = false;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= followRange)
            {
                isFollowingPlayer = true;
                FollowPlayer();
                return;
            }
        }

        if (isFollowingPlayer)
        {
            ReturnToStart();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (movingTowardsEnd)
        {
            transform.position += patrolDirection * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, endPosition) <= 0.1) // changed check here
            {
                movingTowardsEnd = false;
                transform.rotation = Quaternion.LookRotation(-patrolDirection); // Flip the character around
            }
        }
        else
        {
            transform.position -= patrolDirection * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, startPosition) <= 0.1) // changed check here
            {
                movingTowardsEnd = true;
                transform.rotation = Quaternion.LookRotation(patrolDirection); // Flip the character back around
            }
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void ReturnToStart()
    {
        Vector3 direction = startPosition - transform.position;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(startPosition, transform.position) > 0.1f) // If not at the start position
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime); // Move towards start position
        }
        else // If at the start position
        {
            isFollowingPlayer = false; // Resume patrolling
            transform.rotation = startRotation; // Reset to initial rotation
            movingTowardsEnd = true; // Reset patrol direction
        }
    }
}
