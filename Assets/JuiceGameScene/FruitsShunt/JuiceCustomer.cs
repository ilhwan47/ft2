using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JuiceCustomer : MonoBehaviour {

    public int customerIndex;
    public int customerJuiceType;
    public int myLineIndex;
    public float goalLinePositionX;
    public Sprite[] customerTypeImage;
    public float customerMoveSpeed;
    public Sprite[] customerFeeling;
    public float customerCreateTime;
    public float customerCreateTimeSave;
    GameObject[] customerExitPoint = new GameObject[2];
    //GameObject juiceStorePoint;
    GameObject juiceCustomerManager;
    public float waitTime;
    public float waitAngryTime;
    bool getJuice;
    bool timeOver;
    bool coroutineEnd = true;
    int alphaControl = 1;
    public bool customerScreenView;
    public bool waitingLine;
    public bool waitingJuice;
    

    Coroutine angryCustomer;

    int randomExit;

    public CustomerControl _scrCustomerControl;

    // Use this for initialization
    private void Awake()
    {
        
    }

    void Start () {
        //juiceStorePoint = GameObject.Find("juice_store_point");
        juiceCustomerManager = GameObject.Find("JuiceCustomerManager");

        GetComponent<Image>().sprite = customerTypeImage[customerJuiceType];
        randomExit = Random.Range(0, 2);

        customerExitPoint[0] = GameObject.Find("juice_customer_exit_point_1");
        customerExitPoint[1] = GameObject.Find("juice_customer_exit_point_2");

        customerMoveSpeed = Screen.width / 720.0f * customerMoveSpeed;
    }
	
    IEnumerator BlinkImage()
    {
        while(coroutineEnd)
        {
            if(1 <= GetComponent<Image>().color.a)
            {
                GetComponent<Image>().color = new Color(1, 1, 1, GetComponent<Image>().color.a - Time.deltaTime);
                _scrCustomerControl.SetAlpha(new Color(1, 1, 1, GetComponent<Image>().color.a - Time.deltaTime));
            }

            if (0 >= GetComponent<Image>().color.a)
            {
                GetComponent<Image>().color = new Color(1.0f, 1.0f, GetComponent<Image>().color.a + Time.deltaTime);
                _scrCustomerControl.SetAlpha(new Color(1.0f, 1.0f, GetComponent<Image>().color.a + Time.deltaTime));
            }
            yield return null;
        }
    }

	// Update is called once per frame
	void Update () {
        if (getJuice)
        {
            MoveExit();
            return;
        }

        if (0 <= transform.position.x)
        {
            customerScreenView = true;
        }

        if (0 != myLineIndex && !waitingLine)
        {
            transform.Translate(Time.deltaTime * customerMoveSpeed, 0, 0);
            
            if (goalLinePositionX <= transform.position.x)
            {
                transform.position = new Vector3(goalLinePositionX, transform.position.y);
                waitingLine = true;
                waitingJuice = true;
            }
        }
        else if(0 == myLineIndex && !waitingLine)
        {
            transform.Translate(Time.deltaTime * customerMoveSpeed, 0, 0);
            if (goalLinePositionX <= transform.position.x)
            {
                transform.position = new Vector3(goalLinePositionX, transform.position.y);
                transform.localScale *= 1.3f;
                waitingLine = true;
                waitingJuice = true;
            }
        }
        
        if (waitingJuice)
        {
            if (0 >= waitTime)
            {
                coroutineEnd = false;

                GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                _scrCustomerControl.SetAlpha(new Color(1.0f, 1.0f, 1.0f));

                transform.GetChild(0).GetComponent<Text>().text = "";
                _scrCustomerControl.SetWaitTime("");
                timeOver = true;
                FeelingState(customerJuiceType, false);
                juiceCustomerManager.GetComponent<JuiceCustomerManager>().RemoveAtList(this.gameObject);
            }
            else if(waitAngryTime >= waitTime)
            {
                if(0.4f >= GetComponent<Image>().color.a)
                {
                    alphaControl = -1;
                }
                else if (1 <= GetComponent<Image>().color.a)
                {
                    alphaControl = 1;
                }
                GetComponent<Image>().color = new Color(1, 1, 1, GetComponent<Image>().color.a - Time.deltaTime * alphaControl * 2);

                Color color = new Color(1, 1, 1, GetComponent<Image>().color.a - Time.deltaTime * alphaControl * 2);
                _scrCustomerControl.SetAlpha(color);

                transform.GetChild(0).GetComponent<Text>().text = waitTime.ToString("0");
                _scrCustomerControl.SetWaitTime(waitTime.ToString("0"));
                waitTime -= Time.deltaTime;
            }
            else if (0 < waitTime && !timeOver)
            {
                transform.GetChild(0).GetComponent<Text>().text = waitTime.ToString("0");
                _scrCustomerControl.SetWaitTime(waitTime.ToString("0"));
                waitTime -= Time.deltaTime;
            }
        }
	}

    public void FeelingState(int _juiceType, bool _success)
    {
        getJuice = true;
        transform.GetChild(0).GetComponent<Text>().text = "";
        _scrCustomerControl.SetWaitTime("");
        if (customerJuiceType == _juiceType && _success)
        {
            JuiceGameInfoManager.Instance.coolCount++;
            JuiceGameInfoManager.Instance.incPopCount = JuiceGameInfoManager.Instance.coolCount * 1;
            JuiceGameInfoManager.Instance.ComboCheck(true);
            SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_SERVE_COOL);
            GetComponent<Image>().sprite = customerFeeling[1];
            transform.SetAsLastSibling();

            CustomerManager._this.SetCool();
        }
        else
        {
            JuiceGameInfoManager.Instance.badCount++;
            JuiceGameInfoManager.Instance.decPopCount = JuiceGameInfoManager.Instance.badCount * 1;
            JuiceGameInfoManager.Instance.ComboCheck(false);
            SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_SERVE_BED);
            GetComponent<Image>().sprite = customerFeeling[0];
            transform.SetAsLastSibling();

            CustomerManager._this.SetBed();
        }

        transform.localScale = new Vector3(1, 1);
        GetComponent<Image>().SetNativeSize();
    }

    void MoveExit()
    {
        if(transform.position.y <= customerExitPoint[randomExit].transform.position.y)
        {
            MoveExitPointX();
        }
        else
        {
            transform.Translate(0, Time.deltaTime * -customerMoveSpeed, 0);
        }
    }

    void MoveExitPointX()
    {
        if(0 == randomExit)
        {
            if (transform.position.x <= customerExitPoint[randomExit].transform.position.x)
            {
                Destroy(this.gameObject);
            }
            transform.Translate(Time.deltaTime * -customerMoveSpeed, 0, 0);
        }
        else
        {
            if (transform.position.x >= customerExitPoint[randomExit].transform.position.x)
            {
                Destroy(this.gameObject);
            }
            transform.Translate(Time.deltaTime * customerMoveSpeed, 0, 0);
        }
    }

    
}
