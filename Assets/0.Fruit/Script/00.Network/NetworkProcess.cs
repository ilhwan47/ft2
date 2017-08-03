using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkProcess
{
    private static NetworkProcess _instance;

    public static NetworkProcess Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new NetworkProcess();
                _instance.Init();
            }
            return _instance;
        }
    }

    //public static void DestroyInstance()
    //{
    //    instance.Destroy();
    //    instance = null;
    //}

    //public static void CreateInstance()
    //{
    //    instance = new NetworkProcess();
    //    instance.Init();
    //}

    public void Init()
    {
        _listBytes = new List<byte>();
        _listPacket = new List<string>();
    }

    public void Destroy()
    {
        if (null != _Socket)
            _Socket.Close();
        _listBytes.Clear();
        _listBytes = null;
        _listPacket.Clear();
        _listPacket = null;
        _NetworkReceiver = null;
        _instance = null;
    }

    public string _strIP = "ilhwan47.synology.me";
    public int _iPort = 7474;

    private Socket _Socket = null;
    private byte[] _ReceiveBuffer;
    private bool _bReceiveAsyncProcessing = false;
    private List<byte> _listBytes = new List<byte>();
    private List<string> _listPacket = new List<string>();
    INetworkReceiver _NetworkReceiver = null;
    
    private bool ConnectServer(string strHostaddress, int iPort)
    {
        IPAddress serverIp = Dns.GetHostAddresses(strHostaddress)[0];
        IPEndPoint serverEndPoint = new IPEndPoint(serverIp, iPort);

        _ReceiveBuffer = new byte[1024];
        _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            _Socket.Connect(serverEndPoint);
            if (_Socket.Connected)
            {
                Debug.Log(string.Format("Connect Success"));
                CheckReceiveAsync();
                return true;
            }
            else
            {
                Debug.Log(string.Format("Connect Failed"));
                //AddSocketConnectError();
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("Connect Exception : {0}", e));
            //AddSocketConnectError();
            return false;
        }
    }


    public bool CheckConnectServer()
    {
        if (null == _Socket)
            return false;

        return _Socket.Connected;
    }

    public void SendMessage(object message)
    {
        if (CheckConnectServer())
        {
            string packet = JsonUtility.ToJson(message);
            packet = string.Format("{0}#{1}", packet.Length, packet);

            byte[] sendBuffer = new byte[packet.Length];
            sendBuffer = Encoding.UTF8.GetBytes(packet);

            try
            {
                SendPacket(_Socket, sendBuffer, 0, packet.Length, 10000);
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("NetworkProcess.SendMessage : {0}", e.Message));
            }
        }
    }

    public void SendPacket(Socket socket, byte[] buffer, int offset, int size, int timeout)
    {
        int startTickCount = Environment.TickCount;
        int sent = 0;

        do
        {
            if (Environment.TickCount > startTickCount + timeout)
            {
                throw new Exception("Time Out");
            }

            try
            {
                sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
            }
            catch (SocketException e)
            {
                Debug.Log(string.Format("NetworkProcess.SendPacket : {0}", e.Message));
            }
        }
        while (sent < size);
    }
    
    public void CheckReceiveAsync()
    {
        if (!_bReceiveAsyncProcessing)
        {
            SetSoketRecevieCallBack();
        }
    }

    private void SetSoketRecevieCallBack()
    {
        _bReceiveAsyncProcessing = true;
        //if (!ReceiveAsyncProcessing)
        {
            _Socket.BeginReceive(_ReceiveBuffer, 0, _ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveComplete), null);
        }
    }

    private void ReceiveComplete(IAsyncResult ar)
    {
        try
        {
            if (null == _Socket)
                return;

            int len = _Socket.EndReceive(ar);
            if (0 < len)
            {
                byte[] data = new byte[len];
                System.Buffer.BlockCopy(_ReceiveBuffer, 0, data, 0, len);
                _listBytes.AddRange(data);
            }
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("NetworkProcess.ReceiveComplete : {0}", e.Message));
            _listBytes.Clear();
            //AddSocketDisConnectError();
        }

        _bReceiveAsyncProcessing = false;
        CheckReceiveAsync();
    }

    public void CheckByteBuffer()
    {
        string convertedbyte = Encoding.UTF8.GetString(_listBytes.ToArray());
        if (0 < convertedbyte.Length)
        {
            Debug.Log(string.Format("<color=green> >> CheckByteBuffer >> </color> {0}", convertedbyte));

            string[] parsed = convertedbyte.Split(new string[] { "#" }, StringSplitOptions.None);
            if (1 < parsed.Length)
            {
                string lengthstring = parsed[0];
                string body = parsed[1];
                int length = 0;
                int.TryParse(lengthstring, out length);

                if (body.Length >= length && length != 0)
                {
                    string realbody = body.Substring(0, length);
                    try
                    {
                        _listPacket.Add(realbody);
                        int removelength = Encoding.UTF8.GetBytes(lengthstring + "#" + realbody).Length;
                        _listBytes.RemoveRange(0, removelength);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(string.Format("NetworkProcess.CheckByteBuffer : {0}", e.Message));
                    }
                }
                else
                {
                    if (0 == length)
                    {
                        _listBytes.Clear();
                        convertedbyte = "";
                    }
                    else if (2 < parsed.Length)
                    {
                        int lastbraketindex = 0;
                        for (int i = 0; i < body.Length; i++)
                        {
                            if ('}' == body[i])
                            {
                                if (lastbraketindex < i)
                                {
                                    lastbraketindex = i;
                                }
                            }
                        }

                        int removelength = Encoding.UTF8.GetBytes(lengthstring + "#" + body.Substring(0, lastbraketindex + 1)).Length;
                        _listBytes.RemoveRange(0, removelength);

                        if (0 == lastbraketindex)
                        {
                            _listBytes.Clear();
                        }
                    }

                    return;
                }
            }
            else
            {
                _listBytes.Clear();
                convertedbyte = "";
                return;
            }
        }
    }


    void CheckPacketList()
    {
        while (_listPacket.Count > 0)
        {
            string packetData = _listPacket[0];
            
            NetworkMessage message = JsonUtility.FromJson<NetworkMessage>(packetData);
            _NetworkReceiver.ReceiveMessage(message);

            _listPacket.RemoveAt(0);
        }
    }

    public void SetNetworkReceiver(INetworkReceiver NetworkReceiver)
    {
        _NetworkReceiver = NetworkReceiver;
    }

    public void Connect()
    {
        ConnectServer(_strIP, _iPort);
    }
    
    public void Process()
    {
        if (CheckConnectServer())
            CheckReceiveAsync();

        CheckByteBuffer();
        CheckPacketList();
    }
}
