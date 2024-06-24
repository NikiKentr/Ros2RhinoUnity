using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;

public class ROSPosePublisher : MonoBehaviour
{
    public Transform robotBase; // Reference to the Transform representing the robot's base
    public ROSConnection rosConnection; // Reference to the specific ROSConnection instance
    public string poseTopic; // ROS topic to publish pose messages for the robot

    void Start()
    {
        if (rosConnection == null)
        {
            Debug.LogError("ROSConnection is not assigned.");
            return;
        }
        rosConnection.RegisterPublisher<PoseStampedMsg>(poseTopic);
    }

    void Update()
    {
        Vector3 basePosition = robotBase.position;
        Quaternion baseRotation = robotBase.rotation;
        Debug.Log($"Robot Base Position: {basePosition}");
        PublishPoseMessage(basePosition, baseRotation);
    }

    void PublishPoseMessage(Vector3 basePosition, Quaternion baseRotation)
    {
        PoseStampedMsg poseMessage = new PoseStampedMsg
        {
            header = new HeaderMsg
            {
                stamp = new TimeMsg
                {
                    sec = (int)Time.time,
                    nanosec = (uint)((Time.time - Mathf.Floor(Time.time)) * 1e9)
                },
                frame_id = "base_link"
            },
            pose = new PoseMsg
            {
                position = new PointMsg { x = basePosition.x, y = basePosition.z, z = 0.0 },
                orientation = new QuaternionMsg
                {
                    x = 0.0,
                    y = 0.0,
                    z = baseRotation.y,
                    w = baseRotation.w
                }
            }
        };

        rosConnection.Publish(poseTopic, poseMessage);
    }
}