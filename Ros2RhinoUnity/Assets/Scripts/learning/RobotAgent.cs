using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class RobotAgent : Agent
{
    public ArticulationWheelController wheelController;

    public override void OnEpisodeBegin()
    {
        // Reset the environment, reset robot position, target positions, etc.
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add observations here (e.g., robot position, target position)
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Send actions to the wheel controller to move the robot
        
        wheelController.MoveAgent(vectorAction );
        // Implement reward logic based on agent's actions and observations
        // Example:
        // float reward = CalculateReward();
        // AddReward(reward);
        // EndEpisode();
    }

    public override void Heuristic(float[] actionsOut)
    {
        // For manual control (Heuristic mode)
        actionsOut[0] = Input.GetAxisRaw("Vertical");
        actionsOut[1] = -Input.GetAxisRaw("Horizontal");
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Implement observation collection if needed
        // Example: Collect environment state (positions, velocities, etc.) for ML-Agents
    }
}