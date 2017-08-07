using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Juice_m_top : MonoBehaviour {
    public Image[] juice_b_red;
    public Image[] juice_b_yellow;
    public Image[] juice_b_blue;
    public GameObject[] juice_b_anchor_down_top;
    public GameObject[] juice_b_first_second;
    public GameObject[] juice_b;
    public GameObject juice_b_red_top;
    public Image juice_m_mempty;
    public Sprite[] juice_m_m;
    public GameObject juice_b_insert_point;
    public Toggle mixToggle;
    public Text sortAmountText;
    public Text pumpSpeedText;
    


    public bool mixCheck;

    bool[,] thirtyCheck = new bool[3,2];  

    float juice_b_anchorDistanceDT;
    float juice_b_firstMinusSecond;
    float juiceAmountSix;
    float juice_b_insert_positionY;
    float juice_b_original_positionY;
    float containerUpSpeed = 100f;

    int insertedJuiceCount;

    bool[] containerInsertCheck = new bool[3];

    const float juiceSpaceAllAmount = 30.0f;

    Image[,] juice_b_image;

    // Use this for initialization
    void Start()
    {
        juice_b_insert_positionY = juice_b_insert_point.transform.position.y;
        juice_b_original_positionY = juice_b[0].transform.position.y;
        juice_b_image = new Image[3, 2];
        containerInsertCheck = JuiceGameInfoManager.Instance.insertedJuiceCheck;

        for (int i = 0; i < 2; i++)
        {
            juice_b_image[0, i] = juice_b_red[i];
        }
        for (int i = 0; i < 2; i++)
        {
            juice_b_image[1, i] = juice_b_yellow[i];
        }
        for (int i = 0; i < 2; i++)
        {
            juice_b_image[2, i] = juice_b_blue[i];
        }

        juice_b_anchorDistanceDT = juice_b_anchor_down_top[1].transform.position.y - juice_b_anchor_down_top[0].transform.position.y;

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                juice_b_image[i,j].transform.position = new Vector3(juice_b_image[i, j].transform.position.x, juice_b_image[i, j].transform.position.y - juice_b_anchorDistanceDT);
            }
        }

        //juice_b_image[0, 0].transform.position = new Vector3(juice_b_image[0, 0].transform.position.x, juice_b_image[0, 0].transform.position.y - juice_b_anchorDistanceDT);
        //juice_b_image[0, 1].transform.position = new Vector3(juice_b_image[0, 1].transform.position.x, juice_b_image[0, 1].transform.position.y - juice_b_anchorDistanceDT);
        //juice_b_image[1, 0].transform.position = new Vector3(juice_b_image[1, 0].transform.position.x, juice_b_image[1, 0].transform.position.y - juice_b_anchorDistanceDT);
        //juice_b_image[1, 1].transform.position = new Vector3(juice_b_image[1, 1].transform.position.x, juice_b_image[1, 1].transform.position.y - juice_b_anchorDistanceDT);
        //juice_b_image[2, 0].transform.position = new Vector3(juice_b_image[2, 0].transform.position.x, juice_b_image[2, 0].transform.position.y - juice_b_anchorDistanceDT);
        //juice_b_image[2, 1].transform.position = new Vector3(juice_b_image[2, 1].transform.position.x, juice_b_image[2, 1].transform.position.y - juice_b_anchorDistanceDT);

        JuiceGameTextManager.Instance.OutputDedugWindow();
        juice_b_firstMinusSecond = (juice_b_first_second[1].transform.position.y - juice_b_first_second[0].transform.position.y)/3.0f/ 2.0f;
        juiceAmountSix = juice_b_first_second[0].transform.position.y - juice_b_red_top.transform.position.y - (juice_b_firstMinusSecond * 6.0f);
    }

    public void AddJuice(int _fruitsType)
    {
        float tmpJuiceAmount = JuiceGameInfoManager.Instance.juiceAmount[_fruitsType];
        float sortAmountTmp = 2 * JuiceGameInfoManager.Instance.sortAmount;
        float tmpNextJuiceAmount = tmpJuiceAmount + sortAmountTmp;

        if(tmpNextJuiceAmount >= juiceSpaceAllAmount * 2)
        {
            juice_b_image[_fruitsType, 1].transform.Translate(0, ((juiceSpaceAllAmount * 2) - tmpJuiceAmount) * juice_b_firstMinusSecond, 0);
        }
        else if(tmpNextJuiceAmount > juiceSpaceAllAmount && tmpJuiceAmount <= juiceSpaceAllAmount)
        {
            juice_b_image[_fruitsType, 0].transform.Translate(0, (juiceSpaceAllAmount - tmpJuiceAmount) * juice_b_firstMinusSecond, 0);
            juice_b_image[_fruitsType, 1].transform.Translate(0, (tmpNextJuiceAmount - juiceSpaceAllAmount) * juice_b_firstMinusSecond, 0);
        }
        else
        {
            if (juiceSpaceAllAmount < tmpJuiceAmount)
            {
                juice_b_image[_fruitsType, 1].transform.Translate(0, juice_b_firstMinusSecond * 2 * JuiceGameInfoManager.Instance.sortAmount, 0);
            }

            if (juiceSpaceAllAmount > tmpJuiceAmount)
            {
                juice_b_image[_fruitsType, 0].transform.Translate(0, juice_b_firstMinusSecond * 2 * JuiceGameInfoManager.Instance.sortAmount, 0);
            }
        }

        JuiceGameInfoManager.Instance.AddJuice(_fruitsType);
        tmpJuiceAmount = JuiceGameInfoManager.Instance.juiceAmount[_fruitsType];

        if (!thirtyCheck[_fruitsType, 0] && 0 < tmpJuiceAmount)
        {
            juice_b_image[_fruitsType, 0].transform.Translate(0, juiceAmountSix, 0);
            thirtyCheck[_fruitsType, 0] = true;
        }
        else if(!thirtyCheck[_fruitsType,1] && 30 < tmpJuiceAmount)
        {
            juice_b_image[_fruitsType, 1].transform.Translate(0, juiceAmountSix, 0);
            thirtyCheck[_fruitsType, 1] = true;
        }
    }

    public bool MinusJuiceMixUse()
    {
        int[] tmp = new int[3];
        int tmpCount = 0;
        for(int i = 0; i < 3; i++)
        {
            if(containerInsertCheck[i])
            {
                if (0.0f >= JuiceGameInfoManager.Instance.insertedJuiceAmount[i] || JuiceGameInfoManager.Instance.juiceAmount[i] < JuiceGameInfoManager.Instance.insertedJuiceAmount[i])
                {
                    return false;
                }
                else if (0.0f != JuiceGameInfoManager.Instance.insertedJuiceAmount[i] && JuiceGameInfoManager.Instance.juiceAmount[i] >= JuiceGameInfoManager.Instance.insertedJuiceAmount[i])
                {
                    tmp[tmpCount] = i;
                    tmpCount++;
                }
            }
        }

        for(int i = 0; i < tmpCount; i++)
        {
            MinusJuice(tmp[i], JuiceGameInfoManager.Instance.insertedJuiceAmount[tmp[i]]);
        }


        return true;
    }

    public void MinusJuice(int _fruitsType, float _minusValue)
    {
        float tmpJuiceAmount = JuiceGameInfoManager.Instance.juiceAmount[_fruitsType];
        float tmpNextJuiceAmount = tmpJuiceAmount - _minusValue;

        if(tmpNextJuiceAmount < 0)
        {
            juice_b_image[_fruitsType, 0].transform.Translate(0, tmpJuiceAmount * -juice_b_firstMinusSecond, 0);
        }
        else if(tmpNextJuiceAmount < juiceSpaceAllAmount && tmpJuiceAmount >= juiceSpaceAllAmount)
        {
            juice_b_image[_fruitsType, 0].transform.Translate(0, (juiceSpaceAllAmount - tmpNextJuiceAmount) * -juice_b_firstMinusSecond, 0);
            juice_b_image[_fruitsType, 1].transform.Translate(0, (tmpJuiceAmount - juiceSpaceAllAmount) * -juice_b_firstMinusSecond, 0);
        }
        else
        {
            if (juiceSpaceAllAmount < tmpJuiceAmount)
            {
                juice_b_image[_fruitsType, 1].transform.Translate(0, juice_b_firstMinusSecond * -_minusValue, 0);
            }
            else
            {
                juice_b_image[_fruitsType, 0].transform.Translate(0, juice_b_firstMinusSecond * -_minusValue, 0);
            }
        }

        if (thirtyCheck[_fruitsType, 0] && 0 >= tmpJuiceAmount - _minusValue)
        {
            juice_b_image[_fruitsType, 0].transform.Translate(0, -juiceAmountSix, 0);
            thirtyCheck[_fruitsType, 0] = false;
        }
        else if (thirtyCheck[_fruitsType, 1] && 30 >= tmpJuiceAmount - _minusValue)
        {
            juice_b_image[_fruitsType, 1].transform.Translate(0, -juiceAmountSix, 0);
            thirtyCheck[_fruitsType, 1] = false;
        }

        JuiceGameInfoManager.Instance.juiceAmount[_fruitsType] -= _minusValue;
        JuiceGameTextManager.Instance.OutputJuiceAmount();

        if(0 == JuiceGameInfoManager.Instance.juiceAmount[_fruitsType])
        {
            juice_b[_fruitsType].transform.position = new Vector3(juice_b[_fruitsType].transform.position.x, juice_b_original_positionY);

            containerInsertCheck[_fruitsType] = false;

            //if (!containerInsertCheck[0] && !containerInsertCheck[1] && !containerInsertCheck[2])
            //{
            //    juice_m_mempty.GetComponent<Image>().sprite = juice_m_m[7];
            //    //JuiceGameInfoManager.Instance.juice_m_mColor = 7;
            //}
        }
    }
    
    public void ClickJuiceContainer(int _juiceType)
    {
        JuiceMMemptyManage(_juiceType);
        containerInsertCheck[_juiceType] = containerInsertCheck[_juiceType] ? false : true;
        juice_m_mempty.GetComponent<Image>().sprite = containerInsertCheck[_juiceType] ? juice_m_m[_juiceType] : juice_m_m[7];
        JuiceGameInfoManager.Instance.insertedJuiceAmount[_juiceType] = containerInsertCheck[_juiceType] ? 6.0f : 0.0f;

        if (containerInsertCheck[_juiceType])
        {
            juice_b[_juiceType].transform.position = new Vector3(juice_b[_juiceType].transform.position.x, juice_b_insert_positionY);
        }

        for (int i = 0; i < 3; i++)
        {
            if(i != _juiceType && containerInsertCheck[i])
            {
                JuiceGameInfoManager.Instance.insertedJuiceAmount[i] = 0.0f;
                containerInsertCheck[i] = false;
            }
            //if(i == _juiceType)
            //{
            //    juice_b[i].transform.position = new Vector3(juice_b[_juiceType].transform.position.x, juice_b_positionY);
            //}
            //else
            //{
            //    //juice_b[i].transform.position = new Vector3(juice_b[i].transform.position.x, juice_b_original_positionY);
            //    //juice_b[i].transform.GetChild(1).GetComponent<JuiceContainer>().SetMoveUp(true);
            //    JuiceGameInfoManager.Instance.insertedJuiceAmount[i] = 0.0f;
            //    containerInsertCheck[i] = false;
            //}
        }
    }

    public void ClickJuiceContainerMixItemUse(int _juiceType)
    {
        containerInsertCheck[_juiceType] = containerInsertCheck[_juiceType] ? false : true;
        if (containerInsertCheck[_juiceType])
        {
            juice_b[_juiceType].transform.position = new Vector3(juice_b[_juiceType].transform.position.x, juice_b_insert_positionY);
        }
        //float juice_b_positionY = containerInsertCheck[_juiceType] ? juice_b_insert_positionY : juice_b_original_positionY;
        //juice_b[_juiceType].transform.position = new Vector3(juice_b[_juiceType].transform.position.x, juice_b_positionY);

        InsertedJuiceContainerManage(_juiceType);
    }
    
    void JuiceMMemptyManage(int _juiceType)
    {
        JuiceGameInfoManager.Instance.juice_m_mColor = _juiceType;
        juice_m_mempty.GetComponent<Image>().sprite = juice_m_m[_juiceType];
    }

    void InsertedJuiceContainerManage(int _juiceType)
    {
        if (containerInsertCheck[_juiceType])
        {
            insertedJuiceCount++;
        }
        else
        {
            JuiceGameInfoManager.Instance.insertedJuiceAmount[_juiceType] = 0.0f;
            insertedJuiceCount--;
        }

        JuiceMMemptyManage(MixJuiceSprite(containerInsertCheck[0], containerInsertCheck[1], containerInsertCheck[2]));

        for (int i = 0; i < 3; i++)
        {
            if (containerInsertCheck[i])
            {
                JuiceGameInfoManager.Instance.insertedJuiceAmount[i] = 6.0f / insertedJuiceCount;
            }
        }
    }

    public int MixJuiceSprite(bool _r, bool _y, bool _b)
    {
        if(_r && !_y && !_b)
        {
            return 0;
        }
        else if(!_r && _y && !_b)
        {
            return 1;
        }
        else if (!_r && !_y && _b)
        {
            return 2;
        }
        else if (_r && _y && !_b)
        {
            return 3;
        }
        else if (_r && !_y && _b)
        {
            return 4;
        }
        else if (!_r && _y && _b)
        {
            return 5;
        }
        else if (_r && _y && _b)
        {
            return 6;
        }
        else
        {
            return 7;
        }
    }

    public void MixCheck()
    {
        if(!JuiceGameInfoManager.Instance.mixCheck)
        {
            JuiceGameInfoManager.Instance.mixCheck = true;

            for(int i = 0; i < 3; i++)
            {
                if(containerInsertCheck[i])
                {
                    insertedJuiceCount++;
                }
            }
        }
        else
        {
            JuiceGameInfoManager.Instance.mixCheck = false;
            insertedJuiceCount = 0;

            for (int i = 0; i < 3; i++)
            {
                juice_b[i].transform.position = new Vector3(juice_b[i].transform.position.x, juice_b_original_positionY);
                containerInsertCheck[i] = false;
            }

            juice_m_mempty.GetComponent<Image>().sprite = juice_m_m[7];
        }
    }

    public void ClickSortAmount()
    {
        JuiceGameInfoManager.Instance.sortAmount++;

        if (4 == JuiceGameInfoManager.Instance.sortAmount)
        {
            JuiceGameInfoManager.Instance.sortAmount = 1;
        }
        sortAmountText.GetComponent<Text>().text = JuiceGameInfoManager.Instance.sortAmount.ToString();
    }

    public void ClickPumpSpeed()
    {
        JuiceGameInfoManager.Instance.pumpSpeed++;
        if (4 == JuiceGameInfoManager.Instance.pumpSpeed)
        {
            JuiceGameInfoManager.Instance.pumpSpeed = 1.0f;
        }
        pumpSpeedText.text = JuiceGameInfoManager.Instance.pumpSpeed.ToString("0");
    }

    public void SetEmptyImage(int _imageNum)
    {
        juice_m_mempty.GetComponent<Image>().sprite = juice_m_m[7];
        JuiceGameInfoManager.Instance.juice_m_mColor = 7;
    }

    // Update is called once per frame
    void Update () {

        for(int i = 0; i < 3; i++)
        {
            if(!containerInsertCheck[i] && juice_b_original_positionY > juice_b[i].transform.position.y)
            {
                juice_b[i].transform.Translate(0, Time.deltaTime * Screen.width * containerUpSpeed / 720f, 0);
            }
        }
    }
}
