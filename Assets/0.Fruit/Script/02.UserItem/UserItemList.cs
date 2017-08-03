using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItemList : MonoBehaviour
{
    public GameObject _prefabUserItem;
    public GameObject _goGrid;
    public UIPanel _panelScrollView;

    List<GameObject> _listUserItem;

    void Awake()
    {
        _listUserItem = new List<GameObject>();
    }

    public void Open()
    {
        _panelScrollView.transform.localPosition = Vector3.zero;
        _panelScrollView.clipOffset = Vector2.zero;

        StartCoroutine(IEUserItemList());
    }

    public void Close()
    {
        foreach (var go in _listUserItem)
            GameObject.Destroy(go);
    }

    IEnumerator IEUserItemList()
    {
        List<Data.UserItem> list = FruitUserData._this.GetItemList();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = Instantiate(_prefabUserItem);
            go.transform.parent = _goGrid.transform;
            go.transform.localPosition = new Vector3(0f, i * -150f, 0f);
            go.transform.localScale = Vector3.one;

            UserItem scr = go.GetComponent<UserItem>();
            scr.Set(list[i]);

            _listUserItem.Add(go);
        }

        yield break;
    }
}
