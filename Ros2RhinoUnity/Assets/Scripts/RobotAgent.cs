using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;

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
        transform.position = initialRobotPosition;
        transform.rotation = initialRobotRotation;
        targetController.ResetTarget();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
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

        // Reward if robot is at the target point
        //if (targetController.IsRobotAtTargetPoint(transform.position))
        //{
        //    AddReward(1f);
        //    EndEpisode();
       // }

        // End episode if robot tips over or goes out of bounds
        if (transform.position.y < initialRobotPosition.y - 2)
        {
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TargetPoint") == true)
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