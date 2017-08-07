using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitsCreateManager : MonoBehaviour {

    enum FRUITS_COLOR
    {
        FRUITS_RED, FRUITS_YELLOW, FRUITS_BLUE, FRUITS_END
    }

    public List<GameObject> FruitsType;
    public GameObject FruitsBlock;
    GameObject[] fruits_red;
    GameObject[] fruits_blue;
    GameObject[] fruits_yellow;

    //public int[] InvenFruitsCount;
    //public int allFruitsCount;
    int[] fruitsIndex = { 0, 0, 0 };
    int clickCount = 0;
    int inputSlotIndex;

    GameObject slotTmp;// = new GameObject();

    public struct tagBlockInfo
    {
        public Vector3 blockPosition;
        public int blockIndex;
        public int slotIndex;
        public string slotTag;
        public bool select;
        public int fruitsColor;
        public Transform slotTransform;
        public GameObject fruits;

        public tagBlockInfo(Vector3 _blockPosition, int _blockIndex, int _slotIndex, string _slotTag, bool _select, int _fruitsColor, Transform _slotTransform, GameObject _fruits)
        {
            blockPosition = _blockPosition;
            blockIndex = _blockIndex;
            slotIndex = _slotIndex;
            slotTag = _slotTag;
            select = _select;
            fruitsColor = _fruitsColor;
            slotTransform = _slotTransform;
            fruits = _fruits;
        }
    }

    //tagBlockInfo[,] blockInfo = new tagBlockInfo;

    private void Awake()
    {
        Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);

        fruits_red = new GameObject[JuiceGameInfoManager.Instance.fruitsCount[(int)FRUITS_COLOR.FRUITS_RED]];
        for (int i = 0; i < JuiceGameInfoManager.Instance.fruitsCount[(int)FRUITS_COLOR.FRUITS_RED]; i++)
        {
            fruits_red[i] = Instantiate(FruitsType[(int)FRUITS_COLOR.FRUITS_RED], new Vector3(-100.0f, -100.0f, -100), Quaternion.identity);
            fruits_red[i].transform.SetParent(gameObject.transform);
        }
        fruits_yellow = new GameObject[JuiceGameInfoManager.Instance.fruitsCount[(int)FRUITS_COLOR.FRUITS_YELLOW]];
        for (int i = 0; i < JuiceGameInfoManager.Instance.fruitsCount[(int)FRUITS_COLOR.FRUITS_YELLOW]; i++)
        {
            fruits_yellow[i] = Instantiate(FruitsType[(int)FRUITS_COLOR.FRUITS_YELLOW], new Vector3(-100.0f, -100.0f, -100), Quaternion.identity);
            fruits_yellow[i].transform.SetParent(gameObject.transform);
        }

        fruits_blue = new GameObject[JuiceGameInfoManager.Instance.fruitsCount[(int)FRUITS_COLOR.FRUITS_BLUE]];
        for (int i = 0; i < JuiceGameInfoManager.Instance.fruitsCount[(int)FRUITS_COLOR.FRUITS_BLUE]; i++)
        {
            fruits_blue[i] = Instantiate(FruitsType[(int)FRUITS_COLOR.FRUITS_BLUE], new Vector3(-100.0f, -100.0f, -100), Quaternion.identity);
            fruits_blue[i].transform.SetParent(gameObject.transform);
        }

        fruitsIndex[0] = JuiceGameInfoManager.Instance.fruitsCount[0];
        fruitsIndex[1] = JuiceGameInfoManager.Instance.fruitsCount[1];
        fruitsIndex[2] = JuiceGameInfoManager.Instance.fruitsCount[2];

        JuiceGameInfoManager.Instance.allFruitsCount = fruitsIndex[0] + fruitsIndex[1] + fruitsIndex[2];

    }

    // Use this for initialization
    void Start () {
        JuiceGameTextManager.Instance.OutputFruitsCount();
        JuiceGameTextManager.Instance.OutputIceCount();
        JuiceGameTextManager.Instance.OutputSugarCount();
        JuiceGameTextManager.Instance.OutputCupCount();
    }

    

    public void FruitsSelect(int _slotIndex, GameObject _Slot)
    {
        //slotTmp = new GameObject();
        if(0 == clickCount)
        {
            inputSlotIndex = _slotIndex;
            slotTmp = _Slot;
            slotTmp.GetComponent<FruitsSlot>().FruitsSelect();
            clickCount++;
        }
        else if(1 == clickCount)
        {
            if(_slotIndex == inputSlotIndex)
            {
                slotTmp.GetComponent<FruitsSlot>().FruitsUnSelect();
            }
            else
            {
                if(_Slot.GetComponent<FruitsSlot>().FruitsMove(slotTmp.GetComponent<FruitsSlot>().FruitsList()))
                {
                    slotTmp.GetComponent<FruitsSlot>().FruitsUnSelect();
                }
                else
                {

                    if (!_Slot.GetComponent<FruitsSlot>().FruitsBomb())
                    {
                        _Slot.GetComponent<FruitsSlot>().FruitsUnSelect();
                    }
                    
                    slotTmp.GetComponent<FruitsSlot>().SlotFruitsDelete();
                }
            }
            clickCount = 0;
        }
    }

    public GameObject GetFruitsArr(int _fruitsType)
    {
        if((int)FRUITS_COLOR.FRUITS_RED == _fruitsType)
        {
            return fruits_red[fruitsIndex[_fruitsType] - 1];
        }
        else if((int)FRUITS_COLOR.FRUITS_BLUE == _fruitsType)
        {
            return fruits_blue[fruitsIndex[_fruitsType] - 1];
        }
        else
        {
            return fruits_yellow[fruitsIndex[_fruitsType] - 1];
        }
    }

    public void DegreaseFruitsIndex(int _fruitsType)
    {
        fruitsIndex[_fruitsType] -= 1;
    }

    public int GetFruitsIndex(int _fruitsType)
    {
        return fruitsIndex[_fruitsType];
    }

    public int SearchMinFruitsType(int _first_fruitsType, int _second_fruitsType)
    {
        if (fruitsIndex[_second_fruitsType] == 0
            && fruitsIndex[_first_fruitsType] == 0)
        {
            return -1;
        }

        if (fruitsIndex[_first_fruitsType] < fruitsIndex[_second_fruitsType]
            || 0 == fruitsIndex[_first_fruitsType])
        {
            return _second_fruitsType;
        }
        else if (fruitsIndex[_first_fruitsType] > fruitsIndex[_second_fruitsType]
            || 0 == fruitsIndex[_second_fruitsType])
        {
            return _first_fruitsType;
        }
        else if(fruitsIndex[_first_fruitsType] == fruitsIndex[_second_fruitsType])
        {
            if (0 == Random.Range(0, 1))
            {
                return _first_fruitsType;
            }
            else
            {
                return _second_fruitsType;
            }
        }
        else
        {
            return -1;
        }
    }

    // Update is called once per frame
    void Update () {
        if (RuntimePlatform.Android == Application.platform)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
