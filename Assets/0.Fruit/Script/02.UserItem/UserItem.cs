using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItem : MonoBehaviour
{
    public UISprite _sprIcon;
    public UILabel _labelItem;

    public void Set(Data.UserItem item)
    {
        _sprIcon.spriteName = string.Format("fruit_item_{0:00}", item._iItemID);
        _labelItem.text = string.Format("{0}x{1}", item._strName, item._iCount);
    }
}
