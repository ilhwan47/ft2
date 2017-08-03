using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopItemList : MonoBehaviour
{
    public GameObject _prefabItemShopItem;
    public GameObject _goGrid;

    public UIPanel _panelScrollView;
    Vector3 _vecClipOffset = Vector3.zero;

    List<Data.Item> _listFruitDataStructItem;

    List<GameObject> _listItemShopItem;

    void Awake()
    {
        _vecClipOffset = _panelScrollView.clipOffset;

        _listFruitDataStructItem = new List<Data.Item>();
        _listFruitDataStructItem.Add(new Data.Item("딸기", 1, 1, 100));
        _listFruitDataStructItem.Add(new Data.Item("딸기", 1, 10, 1000));
        _listFruitDataStructItem.Add(new Data.Item("레몬", 2, 1, 100));
        _listFruitDataStructItem.Add(new Data.Item("레몬", 2, 10, 1000));
        _listFruitDataStructItem.Add(new Data.Item("포도", 3, 1, 100));
        _listFruitDataStructItem.Add(new Data.Item("포도", 3, 10, 1000));
        _listFruitDataStructItem.Add(new Data.Item("얼음", 4, 1, 100));
        _listFruitDataStructItem.Add(new Data.Item("얼음", 4, 10, 1000));
        _listFruitDataStructItem.Add(new Data.Item("컵", 5, 1, 100));
        _listFruitDataStructItem.Add(new Data.Item("컵", 5, 10, 1000));
        _listFruitDataStructItem.Add(new Data.Item("설탕", 6, 1, 100));
        _listFruitDataStructItem.Add(new Data.Item("설탕", 6, 10, 1000));

        _listItemShopItem = new List<GameObject>();
        _listUserItem = new List<GameObject>();
    }

    public void Open()
    {
        _panelScrollView.transform.localPosition = Vector3.zero;
        _panelScrollView.clipOffset = _vecClipOffset;

        StartCoroutine(IEItemShopItemList());

        OpenUserItem();
    }

    IEnumerator IEItemShopItemList()
    {
        for (int i = 0; i < _listFruitDataStructItem.Count; i++)
        {
            GameObject go = Instantiate(_prefabItemShopItem);
            go.transform.parent = _goGrid.transform;
            go.transform.localPosition = new Vector3(0f, i * -200f, 0f);
            go.transform.localScale = Vector3.one;

            ItemShopItem scr = go.GetComponent<ItemShopItem>();
            scr.Set(_listFruitDataStructItem[i]);

            _listItemShopItem.Add(go);
        }

        yield break;
    }

    public void Close()
    {
        if (null != _listItemShopItem)
            foreach (var go in _listItemShopItem)
                GameObject.Destroy(go);
        CloseUserItem();
    }

    public GameObject _prefabUserItem;
    public GameObject _goGridUserItem;
    public UIPanel _panelScrollViewUserItem;

    List<GameObject> _listUserItem;

    public void OpenUserItem()
    {
        _panelScrollViewUserItem.transform.localPosition = Vector3.zero;
        _panelScrollViewUserItem.clipOffset = Vector2.zero;

        CloseUserItem();
        StartCoroutine(IEUserItemList());
    }
    public void CloseUserItem()
    {
        if (null != _listUserItem)
            foreach (var go in _listUserItem)
                GameObject.Destroy(go);
    }

    IEnumerator IEUserItemList()
    {
        List<Data.UserItem> list = FruitUserData._this.GetItemList();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = Instantiate(_prefabUserItem);
            go.transform.parent = _goGridUserItem.transform;
            go.transform.localPosition = new Vector3(0f, i * -150f, 0f);
            go.transform.localScale = Vector3.one;

            UserItem scr = go.GetComponent<UserItem>();
            scr.Set(list[i]);

            _listUserItem.Add(go);
        }

        yield break;
    }
}
