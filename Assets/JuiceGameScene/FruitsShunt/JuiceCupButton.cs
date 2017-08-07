using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JuiceCupButton : MonoBehaviour {
    public int outputNum;
    public int clickCount;
    public int juiceColor;
    public Sprite[,] redTopDown;
    public Sprite[,] blueTopDown;
    public Sprite[,] yellowTopDown;
    public GameObject Juice_Cup;
    GameObject juice_m_top;

    public void ClickCup()
    {
        //컵 이미지 가져오기
        juiceColor = JuiceGameInfoManager.Instance.juice_m_mColor;

        if(!JuiceGameInfoManager.Instance.insertedJuiceCheck[0] 
            && !JuiceGameInfoManager.Instance.insertedJuiceCheck[1] 
            && !JuiceGameInfoManager.Instance.insertedJuiceCheck[2])
        {
            return;
        }

        if(3 > JuiceGameInfoManager.Instance.iceCount
            || 3 > JuiceGameInfoManager.Instance.sugarCount
            || 0 >= JuiceGameInfoManager.Instance.cupCount)
        {
            return;
        }
        ClickState();
    }

    void ClickState()
    {
        if(JuiceGameInfoManager.Instance.mixCheck)
        {
            if(!juice_m_top.GetComponent<Juice_m_top>().MinusJuiceMixUse())
            {
                return;
            }
        }
        else
        {
            if(6.0f > JuiceGameInfoManager.Instance.juiceAmount[juiceColor])
            {
                return;
            }
            juice_m_top.GetComponent<Juice_m_top>().MinusJuice(juiceColor, 6.0f);
        }

        JuiceGameInfoManager.Instance.pouringJuiceCount++;
        Instantiate(Juice_Cup, transform);
    }

    void ReturnPosition()
    {
        transform.position = transform.parent.position;
    }

    void SetAlpha(float _alpha)
    {
        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, _alpha);
    }

	// Use this for initialization
	void Start () {
        juice_m_top = GameObject.Find("juice_m_top");
    }

    // Update is called once per frame
    void Update () {
    }
}
