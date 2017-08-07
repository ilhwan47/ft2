using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceCustomerManager : MonoBehaviour {

    public int customerCount;
    public GameObject customerCreatePoint;
    public GameObject juiceCustomer;
    LinkedList<GameObject> customer;
    public float customerCreateTime;
    public TextAsset juiceOrder;
    int tradeInfoNum = 0;
    
    int createdCustomerCount = 0;
    float[] linePositionX;

    GameObject juiceStorePoint;

    public CustomerManager _scrCustomerManager;

    public struct tagCustomerInfo
    {
        public int index;
        public int patternNum;
        public int patternIndex;
        public int singleJuiceColor;
        public int multiJuiceColor;

        public tagCustomerInfo(int _index, int _patternNum, int _patternIndex, int _singleJuiceColor, int _multiJuiceColor)
        {
            index = _index;
            patternNum = _patternNum;
            patternIndex = _patternIndex;
            singleJuiceColor = _singleJuiceColor;
            multiJuiceColor = _multiJuiceColor;
        }
    }
    tagCustomerInfo[] customerInfo;
    void LoadJson()
    {
        //TextAsset json = Resources.Load("JuiceOrder") as TextAsset;
        LitJson.JsonData getData = LitJson.JsonMapper.ToObject(juiceOrder.text);

        //dataLength = getData["JuiceOrder"].Count;
        customerInfo = new tagCustomerInfo[getData["JuiceOrder"].Count];


        for (int i = 0; i < getData["JuiceOrder"].Count; ++i)
        {
            customerInfo[i].index = int.Parse(getData["JuiceOrder"][i]["Index"].ToString());
            customerInfo[i].patternNum = int.Parse(getData["JuiceOrder"][i]["PatternNum"].ToString());
            customerInfo[i].patternIndex = int.Parse(getData["JuiceOrder"][i]["PatternIndex"].ToString());
            customerInfo[i].singleJuiceColor = getData["JuiceOrder"][i]["SingleJuiceColor"].ToString().IndexOf("2");
            customerInfo[i].multiJuiceColor = int.Parse(getData["JuiceOrder"][i]["MultiJuiceColor"].ToString());
        }
    }

    public TextAsset tradeDifficulty;
    public struct tagTradeDifficulty
    {
        public int popularity;
        public int customerConstant;
        public float customerCoefficient;
        public int waitTimeConstant;
        public float waitTimeCoefficient;
        public int juicePatternNo;
        public float createTime;
        public float waitTime;

        public tagTradeDifficulty(int _popularity, int _customerConstant, float _customerCoefficient, int _waitTimeConstant, float _waitTimeCoefficient, int _juicePatternNo, float _createTime, float _waitTime)
        {
            popularity = _popularity;
            customerConstant = _customerConstant;
            customerCoefficient = _customerCoefficient;
            waitTimeConstant = _waitTimeConstant;
            waitTimeCoefficient = _waitTimeCoefficient;
            juicePatternNo = _juicePatternNo;
            createTime = _createTime;
            waitTime = _waitTime;
        }
    }
    public tagTradeDifficulty[] tradeDifficultyInfo;
    void LoadTradeDifficulty()
    {
        LitJson.JsonData getData = LitJson.JsonMapper.ToObject(tradeDifficulty.text);
        tradeDifficultyInfo = new tagTradeDifficulty[getData["TradeDifficulty"].Count];

        for (int i = 0; i < getData["TradeDifficulty"].Count; ++i)
        {
            tradeDifficultyInfo[i].popularity = int.Parse(getData["TradeDifficulty"][i]["popularity"].ToString());
            tradeDifficultyInfo[i].customerConstant = int.Parse(getData["TradeDifficulty"][i]["customerConstant"].ToString());
            tradeDifficultyInfo[i].customerCoefficient = float.Parse(getData["TradeDifficulty"][i]["customerCoefficient"].ToString());
            tradeDifficultyInfo[i].waitTimeConstant = int.Parse(getData["TradeDifficulty"][i]["waitTimeConstant"].ToString());
            tradeDifficultyInfo[i].waitTimeCoefficient = float.Parse(getData["TradeDifficulty"][i]["waitTimeCoefficient"].ToString());
            tradeDifficultyInfo[i].juicePatternNo = int.Parse(getData["TradeDifficulty"][i]["wafflePatternNo"].ToString());
            tradeDifficultyInfo[i].createTime = float.Parse(getData["TradeDifficulty"][i]["createTime"].ToString());
            tradeDifficultyInfo[i].waitTime = float.Parse(getData["TradeDifficulty"][i]["waitTime"].ToString());
        }
    }

    private void Awake()
    {
        LoadJson();
    }

    public void RemoveAtList(GameObject _customer)
    {
        int i = 0;

        for (LinkedListNode<GameObject> iter = customer.Find(_customer).Next; iter != customer.Last.Next; iter = iter.Next, i++)
        {
            iter.Value.GetComponent<JuiceCustomer>().myLineIndex--;
            iter.Value.GetComponent<JuiceCustomer>().goalLinePositionX = linePositionX[iter.Value.GetComponent<JuiceCustomer>().myLineIndex];
            iter.Value.GetComponent<JuiceCustomer>().waitingLine = false;
        }

        customer.Remove(_customer);

        if (0 == customer.Count)
        {
            return;
        }
    }

    public bool TargetChange(int _juiceType, bool _juiceSuccess)
    {
        if(0 == customer.Count || !customer.First.Value.GetComponent<JuiceCustomer>().customerScreenView)
        {
            return false;
        }

        customer.First.Value.GetComponent<JuiceCustomer>().FeelingState(_juiceType, _juiceSuccess);
        customer.RemoveFirst();

        if (0 == customer.Count)
        {
            return true;
        }

        int i = 0;
        for(LinkedListNode<GameObject> iter = customer.First; iter != customer.Last.Next; iter = iter.Next, i++)
        {
            iter.Value.GetComponent<JuiceCustomer>().goalLinePositionX = linePositionX[i];
            iter.Value.GetComponent<JuiceCustomer>().myLineIndex = i;
            iter.Value.GetComponent<JuiceCustomer>().waitingLine = false;
        }

        return true;
    }

    public float GetCreateTime(int _index)
    {
        return tradeDifficultyInfo[_index].createTime / 15.0f;
    }

    public GameObject CustomerList()
    {
        return customer.First.Value;
    }

    public tagCustomerInfo GetCustomerInfo(int _index)
    {
        return customerInfo[_index];
    }

    public GameObject _go_juice_store_point;

    // Use this for initialization
    void Start () {
        customer = new LinkedList<GameObject>();
        //juiceStorePoint = GameObject.Find("juice_store_point");
        juiceStorePoint = _go_juice_store_point;
        LoadTradeDifficulty();
        customerCreateTime = tradeDifficultyInfo[tradeInfoNum].createTime/15.0f;// JuiceGameInfoManager.Instance.GetCreateTime(0);

        linePositionX = new float[9];

        float lineDistance = juiceStorePoint.transform.position.x / linePositionX.Length;
        for (int i = 0; i < 9; i++)
        {
            linePositionX[i] = juiceStorePoint.transform.position.x - lineDistance * i;
        }
    }

    // Update is called once per frame
    void Update () {
        customerCreateTime -= Time.deltaTime;

        if (0.0f >= customerCreateTime)
        {
            if(createdCustomerCount == customerCount || linePositionX.Length == customer.Count)
            {
                return;
            }

            customerCreateTime = tradeDifficultyInfo[PlayerInfoManager.Instance.patternNum].createTime / 15.0f;
            GameObject tmpCustomer = new GameObject();

            tmpCustomer = Instantiate(juiceCustomer, customerCreatePoint.transform);


            int iCustomerJuiceType = 0;

            if (JuiceGameInfoManager.Instance.mixCheck)
            {
                if(221 == customerInfo[createdCustomerCount].multiJuiceColor)
                {
                    iCustomerJuiceType = 3;
                    tmpCustomer.GetComponent<JuiceCustomer>().customerJuiceType = 3;
                }
                else if (212 == customerInfo[createdCustomerCount].multiJuiceColor)
                {
                    iCustomerJuiceType = 4;
                    tmpCustomer.GetComponent<JuiceCustomer>().customerJuiceType = 4;
                }
                else if (122 == customerInfo[createdCustomerCount].multiJuiceColor)
                {
                    iCustomerJuiceType = 5;
                    tmpCustomer.GetComponent<JuiceCustomer>().customerJuiceType = 5;
                }
                else if (222 == customerInfo[createdCustomerCount].multiJuiceColor)
                {
                    iCustomerJuiceType = 6;
                    tmpCustomer.GetComponent<JuiceCustomer>().customerJuiceType = 6;
                }
                else
                {
                    iCustomerJuiceType = customerInfo[createdCustomerCount].multiJuiceColor.ToString().IndexOf("2");
                    tmpCustomer.GetComponent<JuiceCustomer>().customerJuiceType = customerInfo[createdCustomerCount].multiJuiceColor.ToString().IndexOf("2");
                }
            }
            else
            {
                iCustomerJuiceType = customerInfo[createdCustomerCount].singleJuiceColor;
                tmpCustomer.GetComponent<JuiceCustomer>().customerJuiceType = customerInfo[createdCustomerCount].singleJuiceColor;
            }

            CustomerControl scr =  _scrCustomerManager.CreateCustomer(iCustomerJuiceType);
            tmpCustomer.GetComponent<JuiceCustomer>()._scrCustomerControl = scr;

            tmpCustomer.GetComponent<JuiceCustomer>().customerIndex = customerInfo[createdCustomerCount].index;
            tmpCustomer.GetComponent<JuiceCustomer>().goalLinePositionX = linePositionX[customer.Count];
            tmpCustomer.GetComponent<JuiceCustomer>().myLineIndex = customer.Count;
            tmpCustomer.GetComponent<JuiceCustomer>().waitTime = tradeDifficultyInfo[PlayerInfoManager.Instance.patternNum].waitTime / 15.0f;
            tmpCustomer.GetComponent<JuiceCustomer>().waitAngryTime = tradeDifficultyInfo[PlayerInfoManager.Instance.patternNum].waitTime / 15.0f * 0.3f;
            customer.AddLast(tmpCustomer);
            customer.Last.Value.transform.SetSiblingIndex(0);
            
            createdCustomerCount++;
        }
    }
}
