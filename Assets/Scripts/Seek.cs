using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class LOSDataSender : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("UDP Settings")]
    public string remoteIP = "127.0.0.1";
    public int remotePort = 5005;

    private UdpClient udpClient;
    private IPEndPoint endPoint;

    void Start()
    {
        udpClient = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
        Debug.Log($"[LOSDataSender] Sending LOS data to {remoteIP}:{remotePort}");
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;
        Vector3 los = toTarget.normalized;
        float distance = toTarget.magnitude;
        float t = Time.time;

        // string msg = string.Format("{0:F3},{1:F4},{2:F4},{3:F4},{4:F2}",
        //     t, los.x, los.y, los.z, distance);
        string msg = string.Format("{0:F4},{1:F4},{2:F4},{3:F2}",
            los.x, los.y, los.z, distance);
        // Send the message over UDP
        byte[] data = Encoding.UTF8.GetBytes(msg);
        int sent = udpClient.Send(data, data.Length, endPoint);
        Debug.Log($"[LOS] Sent {sent} bytes to {endPoint} : {msg}");
    }

    void OnDestroy()
    {
        udpClient.Close();
    }
}
