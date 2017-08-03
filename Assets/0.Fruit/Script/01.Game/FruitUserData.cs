using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FruitUserData : MonoBehaviour
{
    public static FruitUserData _this;

    Data.UserData _UserData;

    void Awake()
    {
        _this = this;
        _UserData = new Data.UserData();
    }

    FruitGameManager.delFunc _delSetGold;

    public void SetGold(FruitGameManager.delFunc delSetGold)
    {
        _delSetGold = delSetGold;
    }

    public int GetGold()
    {
        return _UserData._iGold;
    }

    public void SaveGold(int iGold)
    {
        _UserData._iGold += iGold;
        _delSetGold();
        SaveLocal();
    }

    public void UseGold(int iGold)
    {
        _UserData._iGold -= iGold;
        _delSetGold();
        SaveLocal();
    }

    public void SetItem(Data.Item item)
    {
        Data.UserItem findItem = _UserData._listUserItem.Find(delegate (Data.UserItem element) { return element._iItemID == item._iItemID; });
        if (null == findItem)
            _UserData._listUserItem.Add(new Data.UserItem(item._strName, item._iItemID, item._iCount));
        else
            findItem._iCount += item._iCount;
    }

    public List<Data.UserItem> GetItemList()
    {
        return _UserData._listUserItem;
    }

    public void SaveLocal()
    {
        string strFilename = string.Format("{0}/userdata.json", Application.persistentDataPath);
        byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(_UserData));
        FileStream fs = File.Open(strFilename, FileMode.OpenOrCreate);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }

    public bool LoadLocal()
    {
        string strFilename = string.Format("{0}/userdata.json", Application.persistentDataPath);
        if (File.Exists(strFilename))
        {
            try
            {
                FileStream fs = new FileStream(strFilename, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                if (null != sr)
                {
                    string str = sr.ReadToEnd();
                    _UserData = JsonUtility.FromJson<Data.UserData>(str);
                    sr.Close();
                    _delSetGold();
                    return true;
                }
                fs.Close();
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("{0}", e.Message));
                File.Delete(strFilename);
            }
        }
        return false;
    }
}
