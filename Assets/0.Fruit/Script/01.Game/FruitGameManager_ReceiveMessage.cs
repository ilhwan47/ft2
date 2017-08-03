using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FruitGameManager : MonoBehaviour, INetworkReceiver
{
    public void ReceiveMessage(NetworkMessage message)
    {
        Debug.Log(string.Format("<color=red> >> FruitGameManager.ReceiveMessage : </color> {0}, {1}", (E_SEND_MESSAGE)message._iMessageType, message._Body));

        switch ((E_SEND_MESSAGE)message._iMessageType)
        {
            case E_SEND_MESSAGE.TEST:
                {
                    TestTest test = JsonUtility.FromJson<TestTest>(message._Body);
                    Debug.Log(string.Format(">> FruitGameManager.ReceiveMessage : {0}, {1}, {2}", test._iTest1, test._iTest2, test._strTest));
                }
                break;
        }
    }
}
