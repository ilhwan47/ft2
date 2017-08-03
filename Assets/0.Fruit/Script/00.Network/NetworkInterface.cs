using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkReceiver
{
    void ReceiveMessage(NetworkMessage message);
}

public class NetworkMessage
{
    public int _iMessageType;
    public string _Body;

    public NetworkMessage(int iMessageType, string Body)
    {
        _iMessageType = iMessageType;
        _Body = Body;
    }
}
