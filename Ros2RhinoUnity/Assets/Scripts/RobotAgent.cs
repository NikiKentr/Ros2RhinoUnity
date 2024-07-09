using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RobotAgent : Agent
{
    public ArticulationWheelController wheelController;
    public TargetController targetController;

    private Vector3 initialRobotPosition;
    private Quaternion initialRobotRotation;

    public override void Initialize()
    {
        initialRobotPosition = transform.position;
        initialRobotRotation = transform.rotation;
    }

    public override void OnEpisodeBegin()
    {
        // Reset the robot and the target point
        ResetRobot();
        //targetController.ResetTarget();
    }


    private void ResetRobot()
    {
        // Get the scale of the plane (assuming uniform scaling)
        Vector3 planeScale = Vector3.one * 0.5f;

        // Calculate the actual bounds based on the plane's scale
        Vector3 planeSize = new Vector3(10f * planeScale.x, 0f, 10f * planeScale.z); // Adjust 10f as per your plane's size

        // Calculate the min and max bounds for the robot's position
        Vector3 areaBoundsMin = transform.position - planeSize / 2f;
        Vector3 areaBoundsMax = transform.position + planeSize / 2f;

        // Randomize the robot's position within the specified bounds
        float randomX = Random.Range(areaBoundsMin.x, areaBoundsMax.x);
        float randomZ = Random.Range(areaBoundsMin.z, areaBoundsMax.z);

        // Ensure the robot is placed on the plane at a suitable height (adjust this as needed)
        float yPos = 0.1f; // Adjust this value to the desired height above the plane
        Vector3 randomPosition = new Vector3(randomX, yPos, randomZ);

        // Set the robot's position and rotation
        //transform.position = randomPosition;
        //transform.rotation = initialRobotRotation;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        RaycastHit hit;
        float maxDist = 10f;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDist))
        {
            sensor.AddObservation(hit.distance / maxDist);
        }
        else
        {
            // If no ground detected within maxDist, consider it far away
            sensor.AddObservation(1f);
        }



        // Add robot's position and orientation observations
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);

        // Add the distance to the target
        Vector3 toTarget = targetController.targetPoint.position - transform.position;
        sensor.AddObservation(toTarget.magnitude); // Distance to the target
        sensor.AddObservation(toTarget.normalized); // Direction to the target

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Check the size of continuous actions received
        if (actionBuffers.ContinuousActions.Length < 2)
        {
            Debug.LogError("Expected at least 2 continuous actions but received fewer.");
            return;
        }

        // Continuous actions for linear and angular speeds
        float targetLinearSpeed = actionBuffers.ContinuousActions[0];
        float targetAngularSpeed = actionBuffers.ContinuousActions[1];

        // Move the robot using the wheel controller
        wheelController.MoveAgent(new float[] { targetLinearSpeed, targetAngularSpeed });

        // Reward based on the distance to the target point
        float distanceToTarget = Vector3.Distance(transform.position, targetController.targetPoint.position);
        AddReward(-distanceToTarget / 10f); // Normalize distance reward

        // Small penalty for taking time (to encourage efficiency)
        AddReward(-0.01f);

        // End episode if robot tips over or goes out of bounds
        if (transform.position.y < initialRobotPosition.y - 2)
        {
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TargetPoint"))
        {
            AddReward(1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxisRaw("Vertical"); // Linear speed
        continuousActionsOut[1] = Input.GetAxisRaw("Horizontal"); // Angular speed
    }
}
