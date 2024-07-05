using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public Transform targetArea; // Reference to the target area (plane or quad)
    public Vector3 areaBoundsMin; // Minimum bounds for the area position
    public Vector3 areaBoundsMax; // Maximum bounds for the area position

    private Vector3 targetAreaScale;

    void Start()
    {
        // Store the initial scale of the target area
        targetAreaScale = targetArea.localScale;
        ResetTarget();
    }

    public void ResetTarget()
    {
        // Randomize the target area position within bounds
        float randomX = Random.Range(areaBoundsMin.x, areaBoundsMax.x);
        float randomZ = Random.Range(areaBoundsMin.z, areaBoundsMax.z);
        targetArea.position = new Vector3(randomX, targetArea.position.y, randomZ);
    }

    public bool IsRobotInTargetArea(Vector3 robotPosition)
    {
        Vector3 targetMin = targetArea.position - (targetAreaScale / 2);
        Vector3 targetMax = targetArea.position + (targetAreaScale / 2);

        return robotPosition.x > targetMin.x && robotPosition.x < targetMax.x &&
               robotPosition.z > targetMin.z && robotPosition.z < targetMax.z;
    }
}
