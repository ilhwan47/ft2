using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager _this;
    static int _iCustomerIndex = 0;

    public List<Sprite> _listSprite;

    void Awake()
    {
        _this = this;
    }

    Coroutine _co;

    public void Open()
    {
        //_co = StartCoroutine(IECustomerManager());
    }

    public void Close()
    {
        if (null != _co)
            StopCoroutine(_co);
    }

    public Sprite GetSprite(int iIndex)
    {
        return _listSprite[iIndex];
    }

    IEnumerator IECustomerManager()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CreateCustomer(UnityEngine.Random.Range(0, 3));
            yield return new WaitForSeconds(1f + UnityEngine.Random.Range(0f, 1f));
        }
    }

    public Vector3 _vecCreatePosition;
    public Vector3 _vecCreateAngle;
    public Vector3 _vecTargetPosition;

    public List<GameObject> _listCustomer;

    List<CustomerControl> _listCreateCustomer;

    public enum E_CUSTOMER_STATE { CREATE, MOVE, WAIT, RESULT_GOOD, RESULT_BAD, DESTORY, }
    public enum E_CUSTOMER_TYPE { CHONJANG = 0, DALSOO, MYUNGJANG, }
    public enum E_CUSTOMER_ORDER { RED = 0, YELLOW, BLUE, ORANGE, VIOLET, GREEN, RAINBOW, WAIT_ORDER, }

    public void SetTargetPosition(Vector3 vecTargetPosition)
    {
        _vecTargetPosition = vecTargetPosition;
        _vecCreatePosition = _vecTargetPosition + (Vector3.forward * 25f);
        _vecCreateAngle = new Vector3(0f, 180f, 0f);

        _listCreateCustomer = new List<CustomerControl>();
    }

    public CustomerControl CreateCustomer(int iCustomerJuiceType)
    {
        if (10 > _listCreateCustomer.Count)
        {
            int iCustomerOrder = iCustomerJuiceType;
            int iCustomerType = UnityEngine.Random.Range(0, _listCustomer.Count);
            GameObject go = Instantiate(_listCustomer[iCustomerType]);
            
            go.SetActive(true);
            go.name = string.Format("customer {0:0000}", _iCustomerIndex++);
            go.transform.parent = gameObject.transform;
            go.transform.localPosition = _vecCreatePosition + new Vector3(UnityEngine.Random.Range(-2f, 4f), 0f, UnityEngine.Random.Range(0f, 0.2f));
            go.transform.localEulerAngles = _vecCreateAngle;

            CustomerControl scr = go.GetComponent<CustomerControl>();

            if (0 == _listCreateCustomer.Count)
            {
                scr.Create(_vecTargetPosition + (Vector3.forward * 3f), (E_CUSTOMER_TYPE)iCustomerType, (E_CUSTOMER_ORDER)iCustomerOrder);
            }
            else
            {
                Vector3 vec = _vecTargetPosition + (Vector3.forward * 2f * _listCreateCustomer.Count) + new Vector3(UnityEngine.Random.Range(-0.2f, 0.4f), 0f, UnityEngine.Random.Range(-0.2f, 0.4f));
                scr.Create(vec + (Vector3.forward * 3f), (E_CUSTOMER_TYPE)iCustomerType, (E_CUSTOMER_ORDER)iCustomerOrder);
            }

            _listCreateCustomer.Add(scr);
            return scr;
        }
        return null;
    }

    public void ButtonCreate()
    {
    }

    public void ButtonClose()
    {
        if (null != _listCreateCustomer)
            foreach (var go in _listCreateCustomer)
                GameObject.Destroy(go.gameObject);

        FruitGameManager._this.ButtonStoreClose();
    }

    public void ButtonRed()
    {
        ButtonJuice(E_CUSTOMER_ORDER.RED);
    }

    public void ButtonYellow()
    {
        ButtonJuice(E_CUSTOMER_ORDER.YELLOW);
    }

    public void ButtonBlue()
    {
        ButtonJuice(E_CUSTOMER_ORDER.BLUE);
    }

    public void ButtonJuice(E_CUSTOMER_ORDER eOrder)
    {
        if (0 < _listCreateCustomer.Count)
        {
            if (E_CUSTOMER_STATE.WAIT == _listCreateCustomer[0].GetState())
            {
                if (1 < _listCreateCustomer.Count)
                {
                    for (int i = 1; i < _listCreateCustomer.Count; i++)
                    {
                        Vector3 vec = _vecTargetPosition + (Vector3.forward * 3f);
                        _listCreateCustomer[i].SetAction(vec + (Vector3.forward * 2f * (i - 1)) + new Vector3(UnityEngine.Random.Range(-0.2f, 0.4f), 0f, UnityEngine.Random.Range(-0.2f, 0.4f)), E_CUSTOMER_STATE.MOVE);
                    }
                }

                if (eOrder == E_CUSTOMER_ORDER.WAIT_ORDER)
                {
                    _listCreateCustomer[0].SetAction(_vecTargetPosition + (Vector3.left * 15f), E_CUSTOMER_STATE.RESULT_BAD);
                }
                else
                {
                    if (eOrder == _listCreateCustomer[0].GetOrder())
                    {
                        _listCreateCustomer[0].SetAction(_vecTargetPosition + (Vector3.right * 15f), E_CUSTOMER_STATE.RESULT_GOOD);
                    }
                    else
                    {
                        _listCreateCustomer[0].SetAction(_vecTargetPosition + (Vector3.left * 15f), E_CUSTOMER_STATE.RESULT_BAD);
                    }
                }
                _listCreateCustomer.Remove(_listCreateCustomer[0]);
            }
        }
    }

    public void SetCool()
    {
        if (0 < _listCreateCustomer.Count)
        {
            if (E_CUSTOMER_STATE.WAIT == _listCreateCustomer[0].GetState())
            {
                if (1 < _listCreateCustomer.Count)
                {
                    for (int i = 1; i < _listCreateCustomer.Count; i++)
                    {
                        Vector3 vec = _vecTargetPosition + (Vector3.forward * 3f);
                        _listCreateCustomer[i].SetAction(vec + (Vector3.forward * 2f * (i - 1)) + new Vector3(UnityEngine.Random.Range(-0.2f, 0.4f), 0f, UnityEngine.Random.Range(-0.2f, 0.4f)), E_CUSTOMER_STATE.MOVE);
                    }
                }
                _listCreateCustomer[0].SetAction(_vecTargetPosition + (Vector3.left * 15f), E_CUSTOMER_STATE.RESULT_GOOD);
                _listCreateCustomer.Remove(_listCreateCustomer[0]);
            }
        }
    }

    public void SetBed()
    {
        if (0 < _listCreateCustomer.Count)
        {
            if (E_CUSTOMER_STATE.WAIT == _listCreateCustomer[0].GetState())
            {
                if (1 < _listCreateCustomer.Count)
                {
                    for (int i = 1; i < _listCreateCustomer.Count; i++)
                    {
                        Vector3 vec = _vecTargetPosition + (Vector3.forward * 3f);
                        _listCreateCustomer[i].SetAction(vec + (Vector3.forward * 2f * (i - 1)) + new Vector3(UnityEngine.Random.Range(-0.2f, 0.4f), 0f, UnityEngine.Random.Range(-0.2f, 0.4f)), E_CUSTOMER_STATE.MOVE);
                    }
                }
                _listCreateCustomer[0].SetAction(_vecTargetPosition + (Vector3.left * 15f), E_CUSTOMER_STATE.RESULT_BAD);
                _listCreateCustomer.Remove(_listCreateCustomer[0]);
            }
        }
    }
}
