using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoManager : MonoBehaviour {

    public static PlayerInfoManager _instance = null;

    public int popularity;
    public int patternNum;
    public Text populText;
    public Scrollbar populScroll;
    public int tradeInfoNum;

    private void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public static PlayerInfoManager Instance
    {
        get
        {
            if (null == _instance)
                _instance = GameObject.FindObjectOfType<PlayerInfoManager>();

            return _instance;
        }
    }

    public void PopularityCount()
    {
        popularity = (int)(populScroll.GetComponent<Scrollbar>().value * 10000);
        populText.GetComponent<Text>().text = popularity.ToString();

        ComparePopularInfo();
    }

    public void ClickPopularityUp()
    {
        if(patternNum < 8)
        {
            patternNum++;
        }
        
        //populScroll.GetComponent<Scrollbar>().value = popularity/10000.0f;
        populText.GetComponent<Text>().text = "PatternLv : " + patternNum.ToString();
    }

    public void ClickPopularityDown()
    {
        if(patternNum > 0)
        {
            patternNum--;
        }
        //populScroll.GetComponent<Scrollbar>().value = popularity / 10000.0f;
        populText.GetComponent<Text>().text = "PatternLv : " + patternNum.ToString();

    }

    void ComparePopularInfo()
    {
        
    }

    

    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        

    }
}
