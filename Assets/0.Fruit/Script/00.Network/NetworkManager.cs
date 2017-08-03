using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    void FixedUpdate()
    {
        NetworkProcess.Instance.Process();
    }

    void OnDestroy()
    {
        NetworkProcess.Instance.Destroy();
    }

    public void SetNetworkReceiver(INetworkReceiver NetworkReceiver)
    {
        NetworkProcess.Instance.SetNetworkReceiver(NetworkReceiver);
    }

    public void Connect()
    {
        NetworkProcess.Instance.Connect();
    }
    
    public void Send(object message)
    {
        NetworkProcess.Instance.SendMessage(message);
    }
}