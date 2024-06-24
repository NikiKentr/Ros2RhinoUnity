using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControlRobot2 : MonoBehaviour
{
    public ArticulationWheelController wheelController;

    public float speed = 1.5f;
    public float angularSpeed = 1.5f;
    private float targetLinearSpeed;
    private float targetAngularSpeed;

    public Transform point2; // Reference to the second point

    void Update()
    {
        // Get key input
        targetLinearSpeed = Input.GetAxisRaw("Vertical2") * speed;
        targetAngularSpeed = -Input.GetAxisRaw("Horizontal2") * angularSpeed;

        // Log the positions of the points
        Vector3 point2Position = point2.position;
        Debug.Log($"Point2 Position: {point2Position}");
    }

    void FixedUpdate()
    {
        wheelController.SetRobotVelocity(targetLinearSpeed, targetAngularSpeed);
    }
}