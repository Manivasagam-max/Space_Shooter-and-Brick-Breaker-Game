using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMover : MonoBehaviour
{
 public float initialTargetY = -3f; // Initial position where the formation stops
    public float layerHeight = 1f; // Height of each layer
    public float speed = 0.5f; // Speed of movement

    private bool isMovingFormation = true; // True if the formation is moving to the initial position
    private bool isMovingLayer = false; // True if a layer is moving downward

    void Update()
    {
        // Step 1: Move the formation to the initial position
        if (isMovingFormation)
        {
            MoveFormationToInitialPosition();
        }
        // Step 2: Check if the current layer is destroyed
        else if (!isMovingLayer && IsCurrentLayerDestroyed())
        {
            AdjustCenterPoint();
            MoveToNextLayer();
        }

        // Step 3: Move the next layer downward
        if (isMovingLayer)
        {
            MoveLayerDownward();
        }
    }

    void MoveFormationToInitialPosition()
    {
        // Move the entire formation to the initial target position
        Vector3 targetPosition = new Vector3(transform.position.x, initialTargetY, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Stop when the formation reaches the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMovingFormation = false; // Stop moving the entire formation
        }
    }

    bool IsCurrentLayerDestroyed()
    {
        // Check if all bricks in the current layer are destroyed
        foreach (Transform brick in transform)
        {
            if (brick.gameObject.activeSelf && brick.position.y <= transform.position.y && brick.position.y > transform.position.y - layerHeight)
            {
                return false; // If any brick in the current layer is active, it's not destroyed
            }
        }
        return true; // All bricks in the current layer are destroyed
    }

    void AdjustCenterPoint()
    {
        // Dynamically adjust the center of the formation to the highest unbroken brick
        float highestY = float.MinValue;
        foreach (Transform brick in transform)
        {
            if (brick.gameObject.activeSelf)
            {
                highestY = Mathf.Max(highestY, brick.position.y);
            }
        }

        // Update the formation's center position to match the highest unbroken brick
        if (highestY != float.MinValue)
        {
            transform.position = new Vector3(transform.position.x, highestY, transform.position.z);
        }
    }

    void MoveToNextLayer()
    {
        isMovingLayer = true; // Trigger the movement of the next layer
    }

    void MoveLayerDownward()
    {
        // Target the new position after moving one layer downward
        Vector3 targetPosition = transform.position - new Vector3(0, layerHeight, 0);

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Stop when the target position is reached
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            isMovingLayer = false; // Stop layer-based movement
        }
    }
}
