using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class NewKeyboardControl : MonoBehaviour
{
    private ROSConnection ros;
    public string cmdVelTopic = "cmd_vel"; // The ROS topic to publish twist messages
    public float maxLinearSpeed = 2.0f; // Maximum linear speed in m/s
    public float maxAngularSpeed = 1.0f; // Maximum angular speed in rad/s
    private float linearSpeed = 0.0f; // Current linear speed
    private float angularSpeed = 0.0f; // Current angular speed

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(cmdVelTopic);
    }

    void Update()
    {
        HandleKeyboardInput();
        PublishTwistMessage();
    }

    void HandleKeyboardInput()
    {
        linearSpeed = Input.GetAxis("Vertical") * maxLinearSpeed;
        angularSpeed = -Input.GetAxis("Horizontal") * maxAngularSpeed; // Adjust for negative input for correct direction
    }

    void PublishTwistMessage()
    {
        TwistMsg twistMessage = new TwistMsg
        {
            linear = new Vector3Msg { x = linearSpeed, y = 0, z = 0 },
            angular = new Vector3Msg { x = 0, y = 0, z = angularSpeed }
        };

        ros.Publish(cmdVelTopic, twistMessage);
    }
}
