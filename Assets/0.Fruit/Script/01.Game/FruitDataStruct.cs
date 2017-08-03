using System.Collections;
using System.Collections.Generic;
using System;

public class Data 
{
    [Serializable]
    public class Packet
    {
        public string _strUserName;
        public int _iGold;
        public List<UserItem> _listUserItem;

        public Packet()
        {
            _listUserItem = new List<UserItem>();
        }
    }

    [Serializable]
    public class UserData
    {
        public string _strUserName;
        public int _iGold;
        public List<UserItem> _listUserItem;

        public UserData()
        {
            _listUserItem = new List<UserItem>();
        }
    }

    [Serializable]
    public class UserItem
    {
        public string _strName;
        public int _iItemID;
        public int _iCount;

        public UserItem(string strName, int iItemID, int iCount)
        {
            _strName = strName;
            _iItemID = iItemID;
            _iCount = iCount;
        }
    }
    
    [Serializable]
    public class Item
    {
        public string _strName;
        public int _iItemID;
        public int _iCount;
        public int _iPrice;

        public Item(string strName, int iItemID, int iCount, int iPrice)
        {
            _strName = strName;
            _iItemID = iItemID;
            _iCount = iCount;
            _iPrice = iPrice;
        }

        public Item(Item item)
        {
            _strName = item._strName;
            _iItemID = item._iItemID;
            _iCount = item._iCount;
            _iPrice = item._iPrice;
        }
    }
}
