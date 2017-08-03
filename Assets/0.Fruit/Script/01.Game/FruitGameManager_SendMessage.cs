using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FruitGameManager : MonoBehaviour, INetworkReceiver
{
    public enum E_SEND_MESSAGE { TEST, };
    
    class TestTest
    {
        public int _iTest1;
        public int _iTest2;
        public string _strTest;
    }

    public void NetworkSendMessage(E_SEND_MESSAGE eTpye, params object[] obj)
    {
        Debug.Log(string.Format("<color=yellow> >> FruitGameManager.NetworkSendMessage : </color> {0}", eTpye));

        switch (eTpye)
        {
            case E_SEND_MESSAGE.TEST:
                {
                    TestTest test = new TestTest();
                    test._iTest1 = (int)obj[0];
                    test._iTest2 = (int)obj[1];
                    test._strTest = obj[2].ToString();
                    string body = JsonUtility.ToJson(test);

                    NetworkMessage message = new NetworkMessage((int)E_SEND_MESSAGE.TEST, body);
                    _scrNetworkManager.Send(message);
                }
                break;
        }
    }
}
