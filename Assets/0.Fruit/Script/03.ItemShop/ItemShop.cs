using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public ItemShopChar _scrItemChar;
    public ItemShopItemList _scrItemShopItemList;
    public ItemShopPopup _scrItemPopupYesNo;
    public ItemShopPopup _scrItemPopupOK;
    ItemShopItem _scrItemShopItem;

    public void ButtonOpen()
    {
        gameObject.SetActive(true);
        _scrItemChar.gameObject.SetActive(true);
        _scrItemChar.Open();
        _scrItemShopItemList.Close();
        _scrItemShopItemList.gameObject.SetActive(false);
        _scrItemPopupYesNo.gameObject.SetActive(false);
        _scrItemPopupOK.gameObject.SetActive(false);
    }

    public void ButtonClose()
    {
        gameObject.SetActive(false);
        FruitGameManager._this.CloseUI();
    }

    public void ButtonItemList()
    {
        _scrItemChar.Close();
        _scrItemChar.gameObject.SetActive(false);
        _scrItemShopItemList.gameObject.SetActive(true);
        _scrItemShopItemList.Open();
        _scrItemPopupYesNo.gameObject.SetActive(false);
        _scrItemPopupOK.gameObject.SetActive(false);
    }

    public void ButtonBuyItem(ItemShopItem item)
    {
        if (item.GetItem()._iPrice <= FruitUserData._this.GetGold())
        {
            _scrItemShopItem = item;
            _scrItemPopupYesNo.gameObject.SetActive(true);
            _scrItemPopupYesNo.Open();
            _scrItemPopupOK.gameObject.SetActive(false);
        }
        else
        {
            _scrItemPopupYesNo.gameObject.SetActive(false);
            _scrItemPopupOK.gameObject.SetActive(true);
            _scrItemPopupOK.Open();
        }
    }

    public void ButtonBuyYes()
    {
        FruitUserData._this.UseGold(_scrItemShopItem.GetItem()._iPrice);
        FruitUserData._this.SetItem(_scrItemShopItem.GetItem());
        _scrItemPopupYesNo.gameObject.SetActive(false);
        _scrItemShopItemList.OpenUserItem();
        _scrItemPopupOK.gameObject.SetActive(false);
    }
    
    public void ButtonBuyNo()
    {
        _scrItemPopupYesNo.gameObject.SetActive(false);
        _scrItemPopupOK.gameObject.SetActive(false);
    }
}
