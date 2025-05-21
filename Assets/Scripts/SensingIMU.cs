// NOTE: Fully manual (kinematic) IMU calculation using transform deltas with two-frame spike suppression
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SensingIMU : MonoBehaviour
{
    [Header("UDP Settings")]
    public string remoteIP = "127.0.0.1";
    public int remotePort = 5005;

    private Vector3 lastPosition;
    private Vector3 lastVelocity;
    private Quaternion lastRotation;
    private int fixedFrameCount = 0;
    private UdpClient udpClient;
    private IPEndPoint endPoint;

    void Start()
    {
        // Initialize UDP
        udpClient = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
        Debug.Log($"[SensingIMU] Sending IMU data to {remoteIP}:{remotePort}");
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;
        fixedFrameCount++;

        if (fixedFrameCount == 1)
        {
            // 1st frame: initialize position and rotation only
            lastPosition = currentPosition;
            lastRotation = currentRotation;
            return;
        }
        else if (fixedFrameCount == 2)
        {
            // 2nd frame: initialize velocity, then skip sending
            lastVelocity = (currentPosition - lastPosition) / dt;
            lastPosition = currentPosition;
            lastRotation = currentRotation;
            return;
        }

        // 3rd+ frames: calculate IMU normally
        // 1) Manual velocity and acceleration (no physics)
        Vector3 currentVelocity = (currentPosition - lastPosition) / dt;
        Vector3 acceleration = (currentVelocity - lastVelocity) / dt;
        Vector3 localAccel = transform.InverseTransformDirection(acceleration);

        // 2) Manual angular velocity from rotation delta
        Quaternion deltaRot = currentRotation * Quaternion.Inverse(lastRotation);
        deltaRot.ToAngleAxis(out float angleDeg, out Vector3 axis);
        if (angleDeg > 180f) angleDeg -= 360f;
        Vector3 angularVelocity = axis.normalized * (angleDeg * Mathf.Deg2Rad / dt);
        Vector3 localGyro = transform.InverseTransformDirection(angularVelocity);

        // 3) Store for next frame
        lastPosition = currentPosition;
        lastVelocity = currentVelocity;
        lastRotation = currentRotation;

        // 4) UDP transmission: time, ax, ay, az, gx, gy, gz
        float t = Time.fixedTime; // Use fixedTime to match FixedUpdate intervals
        string msg = string.Format("{0:F3},{1:F4},{2:F4},{3:F4},{4:F4},{5:F4},{6:F4}",
            t, localAccel.x, localAccel.y, localAccel.z,
            localGyro.x, localGyro.y, localGyro.z);
        byte[] data = Encoding.UTF8.GetBytes(msg);
        udpClient.Send(data, data.Length, endPoint);
    }

    void OnDestroy()
    {
        udpClient.Close();
    }
}
