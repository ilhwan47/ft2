using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopItem : MonoBehaviour
{
    public UISprite _sprIcon;
    public UILabel _labelItem;
    public UILabel _labelPrice;

    Data.Item _item;

    public void Set(Data.Item item)
    {
        _item = item;

        _sprIcon.spriteName = string.Format("fruit_item_{0:00}", item._iItemID);
        _labelItem.text = string.Format("{0}x{1}", item._strName, item._iCount);
        _labelPrice.text = string.Format("{0} 원", item._iPrice);
    }

    public Data.Item GetItem()
    {
        return _item;
    }
}
