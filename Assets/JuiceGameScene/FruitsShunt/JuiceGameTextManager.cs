using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JuiceGameTextManager : MonoBehaviour {
    public static JuiceGameTextManager _instance = null;
    public Text fruitsCount;
    public Text iceCount;
    public Text sugarCount;
    public Text cupCount;
    public Text JuiceAmountTest;
    public Text DebugWindow;

    private void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public static JuiceGameTextManager Instance
    {

        get
        {
            if (null == _instance)
                _instance = GameObject.FindGameObjectWithTag("InfoManager").GetComponent<JuiceGameTextManager>();


            return _instance;
        }
    }

    public void OutputJuiceAmount()
    {
        JuiceAmountTest.GetComponent<Text>().text = "R : " + JuiceGameInfoManager.Instance.juiceAmount[0].ToString() + "  "
                                                    + "Y : " + JuiceGameInfoManager.Instance.juiceAmount[1].ToString() + "  "
                                                    + "B : " + JuiceGameInfoManager.Instance.juiceAmount[2].ToString();
    }

    public void OutputFruitsCount()
    {
        fruitsCount.GetComponent<Text>().text = JuiceGameInfoManager.Instance.allFruitsCount.ToString();
    }

    public void OutputIceCount()
    {
        iceCount.GetComponent<Text>().text = JuiceGameInfoManager.Instance.iceCount.ToString();
    }

    public void OutputSugarCount()
    {
        sugarCount.GetComponent<Text>().text = JuiceGameInfoManager.Instance.sugarCount.ToString();
    }

    public void OutputCupCount()
    {
        cupCount.GetComponent<Text>().text = JuiceGameInfoManager.Instance.cupCount.ToString();
    }

    public void OutputDedugWindow()
    {
        //DebugWindow.text = (Screen.width * 100f / 720f).ToString();
    }

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
    }
}
