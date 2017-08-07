using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceGameInfoManager : MonoBehaviour {

    public static JuiceGameInfoManager _instance = null;

    enum FRUITS_COLOR
    {
        FRUITS_RED, FRUITS_YELLOW, FRUITS_BLUE, FRUITS_END
    }

    public int[] fruitsCount;// = new int[(int)FRUITS_COLOR.FRUITS_END];
    public int allFruitsCount;
    public int[] deadFruitsCount;
    public float[] juiceAmount;
    public int iceCount;
    public int sugarCount;
    public int cupCount;
    public int juice_m_mColor;
    public int firstJuiceCustomerType;
    public int firstJuiceCustomerIndex;
    public int firstJuiceCustomerFeeling;
    public bool mixCheck;
    public bool[] insertedJuiceCheck = new bool[3];
    public float[] insertedJuiceAmount = new float[3];
    public int tradeInfoIndex;
    public float pumpSpeed = 1.0f;
    public bool juicem_memptyCheck;
    public int sortAmount = 1;

    public int pouringJuiceCount = 0;

    public enum COMBOSTATE
    {
        COMBO_START, COMBO_STOP, END
    }
    public int coolCount;
    public int badCount;
    public int incPopCount;
    public int decPopCount;
    public int combo;
    public int[] maxComboCompare = new int[2];
    public int maxCombo;
    int maxComboIndex = 0;

    public int[] popularity = new int[2]; 

    COMBOSTATE comboState;


    //public TextAsset popularityLevel;

    //struct tagPopularityLevel
    //{
    //    public int level;
    //    public int popularity;
    //    public int maxCustomers;

    //    public tagPopularityLevel(int _level, int _popularity, int _maxCustomers)
    //    {
    //        level = _level;
    //        popularity = _popularity;
    //        maxCustomers = _maxCustomers;
    //    }
    //}

    //tagPopularityLevel[] popularityLevelInfo;

    //// Use this for initialization
    //void LoadPopularityLevel()
    //{
    //    LitJson.JsonData getData = LitJson.JsonMapper.ToObject(popularityLevel.text);

    //    //dataLength = getData["JuiceOrder"].Count;
    //    popularityLevelInfo = new tagPopularityLevel[getData["PopularityLevel"].Count];


    //    for (int i = 0; i < getData["PopularityLevel"].Count; ++i)
    //    {
    //        popularityLevelInfo[i].level = int.Parse(getData["PopularityLevel"][i]["Index"].ToString());
    //        popularityLevelInfo[i].popularity = int.Parse(getData["PopularityLevel"][i]["PatternNum"].ToString());
    //        popularityLevelInfo[i].maxCustomers = int.Parse(getData["PopularityLevel"][i]["PatternIndex"].ToString());
    //    }
    //}

    void Start()
    {
        juice_m_mColor = 7;
        juiceAmount = new float[3];
        comboState = COMBOSTATE.COMBO_STOP;
    }

    private void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
        }
        //else
        //{
        //    Destroy(this.gameObject);
        //}

        DontDestroyOnLoad(this.gameObject);

        //LoadTradeDifficulty();
    }

    public static JuiceGameInfoManager Instance
    {
        get
        {
            if (null == _instance)
                _instance = GameObject.FindObjectOfType<JuiceGameInfoManager>();

            return _instance;
        }
    }

    // Use this for initialization

    public void MinusFruitsCount(int _fruitsType)
    {
        fruitsCount[_fruitsType] -= 1;

        allFruitsCount = fruitsCount[0] + fruitsCount[1] + fruitsCount[2];
    }

    public void AddJuice(int _fruitsType)
    {
        juiceAmount[_fruitsType] += 2.0f * sortAmount;

        if (juiceAmount[_fruitsType] > 60)
        {
            juiceAmount[_fruitsType] = 60;
        }

        JuiceGameTextManager._instance.OutputJuiceAmount();
    }

    public bool AlertEmptyFruits()
    {
        if (3 > fruitsCount[0] || 3 > fruitsCount[1] || 3 > fruitsCount[2])
        {
            return true;
        }
        return false;
    }

    public void ComboCheck(bool _comboCheck)
    {
        if(_comboCheck)
        {
            popularity[0]++;
            if (COMBOSTATE.COMBO_STOP == comboState)
            {
                comboState = COMBOSTATE.COMBO_START;
            }
            else if (COMBOSTATE.COMBO_START == comboState)
            {
                SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_SERVE_COMBO);
                combo++;

                maxComboCompare[maxComboIndex]++;
                maxCombo = maxComboCompare[0] >= maxComboCompare[1] ? maxComboCompare[0] : maxComboCompare[1];
            }
        }
        else
        {
            popularity[1]++;
            comboState = COMBOSTATE.COMBO_STOP;
            maxComboIndex = maxComboCompare[0] >= maxComboCompare[1] ? 1 : 0;
            maxComboCompare[maxComboIndex] = 0;
        }
    }
        // Update is called once per frame
    void Update()
    {

    }
}
