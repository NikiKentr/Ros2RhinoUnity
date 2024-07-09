using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public Transform targetPoint; // Reference to the target point
    public Vector3 areaBoundsMin; // Minimum bounds for the target position
    public Vector3 areaBoundsMax; // Maximum bounds for the target position

    void Start()
    {
       // ResetTarget();
    }

    //public void ResetTarget()
    //{
        // Randomize the target point position within bounds
    //    float randomX = Random.Range(areaBoundsMin.x, areaBoundsMax.x);
    //    float randomZ = Random.Range(areaBoundsMin.z, areaBoundsMax.z);
     //   targetPoint.position = new Vector3(randomX, targetPoint.position.y, randomZ);
    //}

    public bool IsRobotAtTargetPoint(Vector3 robotPosition, float threshold = 0.5f)
    {
        // Check if the robot is within the threshold distance to the target point
        float distance = Vector3.Distance(robotPosition, targetPoint.position);
        return distance <= threshold;
    }

}