using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JuiceCup : MonoBehaviour {

    public Sprite[] cupColor;
    public Sprite[] cupTopSprite;
    public Sprite[] cupJuiceColor;
    public GameObject[] cupAnchorTopDown;
    public GameObject cupMask;
    GameObject juice_m_top;
    
    public float juicePouringSpeed;

    GameObject customer;
    GameObject juiceDeleteSpace;

    //Vector3 originalPos;
    int juiceType;
    bool pourStop;
    bool dragCheck;
    //bool secondClick;
    bool juiceSuccess;
    bool initOnce;
    float juiceAmountMinusTime;
    float cupAnchorDistance;
    float presentJuiceAmountOfCup;
    float juiceOverAmountTime = 3.0f;
    float deletePositionY;
    public float shakeSpeed;
    float shakeTime;

	// Use this for initialization
	void Start () {
        if(0 == transform.childCount)
        {
            return;
        }
        customer = GameObject.Find("JuiceCustomerManager");
        juiceDeleteSpace = GameObject.Find("juice_delete_space");
        deletePositionY = juiceDeleteSpace.transform.position.y;
        juiceType = JuiceGameInfoManager.Instance.juice_m_mColor;
        juice_m_top = GameObject.Find("juice_m_top");

        cupMask.transform.GetChild(0).GetComponent<Image>().sprite = cupColor[juiceType];
        cupMask.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = cupTopSprite[juiceType];

        transform.GetChild(2).GetComponent<Image>().sprite = cupJuiceColor[juiceType];

        cupAnchorDistance = cupAnchorTopDown[1].transform.position.y - cupAnchorTopDown[0].transform.position.y;
        cupMask.transform.GetChild(0).transform.position = new Vector3(cupMask.transform.GetChild(0).transform.position.x, cupMask.transform.GetChild(0).transform.position.y - cupAnchorDistance + cupAnchorDistance/4);


        JuiceGameInfoManager.Instance.iceCount -= 3;
        JuiceGameInfoManager.Instance.cupCount -= 1;
        JuiceGameInfoManager.Instance.sugarCount -= 3;

        JuiceGameTextManager.Instance.OutputIceCount();
        JuiceGameTextManager.Instance.OutputSugarCount();
        JuiceGameTextManager.Instance.OutputCupCount();
        JuiceGameTextManager.Instance.OutputJuiceAmount();

        SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_JUICE_FILLSTART);
    }

    public void Dead()
    {
        JuiceGameInfoManager.Instance.pouringJuiceCount--;
        if(0 == JuiceGameInfoManager.Instance.pouringJuiceCount 
            && !JuiceGameInfoManager.Instance.insertedJuiceCheck[0]
            && !JuiceGameInfoManager.Instance.insertedJuiceCheck[1]
            && !JuiceGameInfoManager.Instance.insertedJuiceCheck[2])
        {
            juice_m_top.GetComponent<Juice_m_top>().SetEmptyImage(7);
        }
        Destroy(this.gameObject);
    }

    public void OnMove()
    {
        //if(pourStop)
        {
            GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x, Input.mousePosition.y);

            if (deletePositionY >= transform.position.y)
            {
                juiceDeleteSpace.GetComponent<Image>().color = new Color(1,1,1,1);
                dragCheck = true;

            }
            else
            {
                juiceDeleteSpace.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void OnUp()
    {
        if (!pourStop)
        {
            if (dragCheck)
            {
                juiceDeleteSpace.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_JUICE_DISCARD);
                Dead(); //쓰레기통에 버리는부분
                return;
            }

            pourStop = true; //붓는거 멈춤

            transform.GetChild(2).GetComponent<Image>().color = new Color(transform.GetChild(1).GetComponent<Image>().color.b,
                                                                        transform.GetChild(1).GetComponent<Image>().color.g,
                                                                        transform.GetChild(1).GetComponent<Image>().color.b, 0);
            transform.position = transform.parent.position;

            SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_JUICE_FILLSTOP);

            return;
        }

        if(dragCheck)
        {
            juiceDeleteSpace.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_JUICE_DISCARD);
            Dead(); //쓰레기통에 버리는부분
            return;
        }
        else
        {
            juiceDeleteSpace.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            JuiceGameInfoManager.Instance.firstJuiceCustomerType = juiceType;
            if (customer.GetComponent<JuiceCustomerManager>().TargetChange(juiceType, juiceSuccess))
            {
                Dead(); //손님한테주는부분
            }
            else
            {
                transform.position = transform.parent.position;
            }
        }
    }

    public void OnDown()
    {
        
    }

    void PouringStop()
    {
        if(!cupMask)
        {
            return;
        }

        if(pourStop)
        {
            if(juiceSuccess)
            {
                transform.GetChild(3).GetComponent<Image>().color = new Color(transform.GetChild(1).GetComponent<Image>().color.b,
                                                                        transform.GetChild(1).GetComponent<Image>().color.g,
                                                                        transform.GetChild(1).GetComponent<Image>().color.b, 1);
            }
            else
            {

            }
        }
        else
        {
            if (cupAnchorDistance * 3 / 4 <= presentJuiceAmountOfCup)
            {
                if (0 >= juiceOverAmountTime)
                {
                    juiceSuccess = false;

                    if (!initOnce)
                    {
                        SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_JUICE_OVERFLOW);
                        cupMask.transform.GetChild(0).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
                        cupMask.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);

                        pourStop = true;
                        transform.GetChild(2).GetComponent<Image>().color = new Color(transform.GetChild(1).GetComponent<Image>().color.b,
                                                                        transform.GetChild(1).GetComponent<Image>().color.g,
                                                                        transform.GetChild(1).GetComponent<Image>().color.b, 0);

                        transform.position = transform.parent.position;
                        initOnce = true;
                    }
                }
                else
                {
                    juiceOverAmountTime -= Time.deltaTime;

                    shakeTime += Time.deltaTime;

                    if(transform.parent.position.x + Screen.width * 5.0f / 720.0f <= transform.position.x)
                    {
                        shakeSpeed *= -1;
                    }
                    else if (transform.parent.position.x - Screen.width * 5.0f / 720.0f >= transform.position.x)
                    {
                        shakeSpeed *= -1;
                    }

                    transform.Translate(Time.deltaTime * Screen.width * shakeSpeed / 720f, 0, 0);

                    //if (!shake)
                    //{
                    //    if(transform.parent.position.x +  <= transform.position.x)
                    //    {

                    //    }
                    //}

                    //if (0.1f <= shakeTime)
                    //{
                    //    if (!shake)
                    //    {
                    //        transform.position = transform.parent.position;
                    //        transform.Translate(Time.deltaTime * Screen.width * shakeSpeed / 720.0f, 0, 0);
                    //        shakeTime = 0.0f;
                    //        shake = true;
                    //    }
                    //    else
                    //    {
                    //        transform.position = transform.parent.position;
                    //        transform.Translate(-Time.deltaTime * Screen.width * shakeSpeed / 720.0f, 0, 0);
                    //        shakeTime = 0.0f;
                    //        shake = false;
                    //    }
                    //}

                    cupMask.transform.GetChild(0).transform.position = cupMask.transform.position;
                    transform.GetChild(2).transform.position = new Vector3(transform.parent.position.x, transform.GetChild(2).transform.position.y);
                    juiceSuccess = true;

                }
                return;
            }

            presentJuiceAmountOfCup += Time.deltaTime * cupAnchorDistance / juicePouringSpeed * JuiceGameInfoManager.Instance.pumpSpeed;
            juiceAmountMinusTime += Time.deltaTime * cupAnchorDistance / juicePouringSpeed * JuiceGameInfoManager.Instance.pumpSpeed;

            cupMask.transform.GetChild(0).transform.Translate(0, Time.deltaTime * cupAnchorDistance / juicePouringSpeed * JuiceGameInfoManager.Instance.pumpSpeed, 0);
            transform.GetChild(2).transform.Translate(0, Time.deltaTime * cupAnchorDistance / juicePouringSpeed * JuiceGameInfoManager.Instance.pumpSpeed, 0);
        }
    }

    

    // Update is called once per frame
    void Update () {
        PouringStop();
    }
}
