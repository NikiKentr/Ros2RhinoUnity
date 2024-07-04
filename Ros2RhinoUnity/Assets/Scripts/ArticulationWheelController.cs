using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This script converts linear velocity and 
///     angular velocity to joint velocities for
///     differential drive robot.
/// </summary>
public class ArticulationWheelController : MonoBehaviour
{
    public ArticulationBody leftWheel;
    public ArticulationBody rightWheel;
    public float wheelTrackLength;
    public float wheelRadius;

    private float vRight;
    private float vLeft;

    void Start() {}

    void Update() {}

    public void MoveAgent(float[] actions)
    {
        float targetLinearSpeed = actions[0]; // linear speed
        float targetAngularSpeed = actions[1]; // angular speed

        // Stop the wheels if target velocities are zero
        if (targetLinearSpeed == 0 && targetAngularSpeed == 0)
        {
            StopWheel(leftWheel);
            StopWheel(rightWheel);
        }
        else
        {
            // Convert linear and angular velocities to wheel speeds
            vRight = targetAngularSpeed * (wheelTrackLength / 2) + targetLinearSpeed;
            vLeft = -targetAngularSpeed * (wheelTrackLength / 2) + targetLinearSpeed;

            // Set wheel velocities
            SetWheelVelocity(leftWheel, vLeft / wheelRadius * Mathf.Rad2Deg);
            SetWheelVelocity(rightWheel, vRight / wheelRadius * Mathf.Rad2Deg);
        }
    }


    private void SetWheelVelocity(ArticulationBody wheel, float jointVelocity)
    {
        ArticulationDrive drive = wheel.xDrive;
        drive.target = drive.target + jointVelocity * Time.fixedDeltaTime;
        wheel.xDrive = drive;
    }

    private void StopWheel(ArticulationBody wheel)
    {
        ArticulationDrive drive = wheel.xDrive;
        drive.target = wheel.jointPosition[0] * Mathf.Rad2Deg; // Maintain current angle
        wheel.xDrive = drive;
    }
}