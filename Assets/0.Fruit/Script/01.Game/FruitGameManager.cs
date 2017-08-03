using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FruitGameManager : MonoBehaviour, INetworkReceiver
{
    public static FruitGameManager _this;

    public NetworkManager _scrNetworkManager;

    public GameObject _EventSystem;
    public JoystickHandler _Joystick;
    public FruitPlayer _Player;

    public UserItemList _scrUserItemList;
    public ItemShop _scrItemShop;
    public StoreManager _scrStoreManager;
    public CustomerManager _scrCustomerManager;

    public UILabel _labelGold;

    public delegate void delOpenUI(GameObject go);
    public delegate void delFunc();

    private void Awake()
    {
        _this = this;
    }

    private void Start()
    {
        _Player.SetDelegate(OpenUI);
        _scrItemShop.ButtonClose();
        _scrStoreManager.gameObject.SetActive(false);
        _scrCustomerManager.Close();

        FruitUserData._this.SetGold(UpdateGold);

        if (!FruitUserData._this.LoadLocal())
            FruitUserData._this.SaveGold(10000);

        _scrNetworkManager.Connect();
        _scrNetworkManager.SetNetworkReceiver(this);
    }

    public void UpdateGold()
    {
        _labelGold.text = string.Format("{0} 원", FruitUserData._this.GetGold().ToString("n0"));
    }

    public void OpenUI(GameObject go)
    {
        if (0 == go.name.CompareTo("ItemShop"))
        {
            _Joystick.OnPointerUp(null);
            _EventSystem.SetActive(false);
            _scrItemShop.ButtonOpen();
        }
        else if (0 == go.name.CompareTo("ShopJuice"))
        {
            _Player.transform.localPosition = go.transform.localPosition;
            _Player.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            _scrCustomerManager.SetTargetPosition(_Player.transform.localPosition);
            go.SetActive(false);
            _Joystick.OnPointerUp(null);
            _EventSystem.SetActive(false);
            ButtonStoreOpen();
        }
    }

    public void CloseUI()
    {
        _EventSystem.SetActive(true);
    }

    public void ButtonOpenUserInfo()
    {
        _Joystick.OnPointerUp(null);
        _EventSystem.SetActive(false);
        _scrUserItemList.gameObject.SetActive(true);
        _scrUserItemList.Open();
    }

    public void ButtonCloseUserInfo()
    {
        _EventSystem.SetActive(true);
        _scrUserItemList.Close();
        _scrUserItemList.gameObject.SetActive(false);
    }

    public void ButtonStoreOpen()
    {
        _scrStoreManager.gameObject.SetActive(true);
        _scrCustomerManager.Open();
    }

    public void ButtonStoreClose()
    {
        _EventSystem.SetActive(true);
        _scrStoreManager.gameObject.SetActive(false);
        _scrCustomerManager.Close();
    }

    public void ButtonTest()
    {
        NetworkSendMessage(E_SEND_MESSAGE.TEST, 47, 8047, "ilhwan47");
    }
}
