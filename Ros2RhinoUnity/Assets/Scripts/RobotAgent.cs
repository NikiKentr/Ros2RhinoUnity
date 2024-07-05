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
        // reset the robot and the target area
        transform.position = initialRobotPosition;
        transform.rotation = initialRobotRotation;
        targetController.ResetTarget();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // add robot's position and orientation
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);

        // add target area position relative to the robot
        Vector3 relativeTargetPosition = targetController.targetArea.position - transform.position;
        sensor.AddObservation(relativeTargetPosition);

       
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // continuous actions for linear and angular speeds
        float targetLinearSpeed = actionBuffers.ContinuousActions[0];
        float targetAngularSpeed = actionBuffers.ContinuousActions[1];

        // Move the robot using the wheel controller
        wheelController.MoveAgent(new float[] { targetLinearSpeed, targetAngularSpeed });

        // reward based on the distance to the target area
        float distanceToTarget = Vector3.Distance(transform.position, targetController.targetArea.position);
        AddReward(-distanceToTarget / 10f); // Normalize distance reward

        // small penalty for taking time (to encourage efficiency)
        AddReward(-0.01f);

        // reward if robot is within the target area
        if (targetController.IsRobotInTargetArea(transform.position))
        {
            AddReward(1f);
            EndEpisode();
        }

        // end episode if robot tips over or goes out of bounds
        if (transform.position.y < initialRobotPosition.y - 2)
        {
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