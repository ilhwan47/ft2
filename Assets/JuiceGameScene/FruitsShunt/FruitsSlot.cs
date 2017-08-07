using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitsSlot : MonoBehaviour
{
    public int slotNum;
    public GameObject fruitsCreateMgr;
    float fruitsDistance;
    float fruitsStartY;
    float fruitsEndY;

    public int blockLength = 0;
    LinkedList<GameObject> slotFruitsList;
    tagBlockInfo[] blockInfo;

    enum FRUITS_COLOR
    {
        FRUITS_RED, FRUITS_YELLOW, FRUITS_BLUE, FRUITS_END
    }

    public struct tagBlockInfo
    {
        public Vector3 blockPosition;
        public int blockIndex;
        public int slotIndex;

        public tagBlockInfo(Vector3 _blockPosition, int _blockIndex, int _slotIndex)
        {
            blockPosition = _blockPosition;
            blockIndex = _blockIndex;
            slotIndex = _slotIndex;
        }
    }

    void BlockInfoCreate()
    {
        blockInfo = new tagBlockInfo[9];
        fruitsStartY = transform.GetChild(0).GetComponent<RectTransform>().position.y;
        fruitsEndY = transform.GetChild(1).GetComponent<RectTransform>().position.y;
        fruitsDistance = (fruitsStartY - fruitsEndY) / 9;

        for (int i = 0; i < 9; i++)
        {
            blockInfo[i].blockPosition = new Vector3(transform.position.x, fruitsStartY - fruitsDistance * i, 0);
            blockInfo[i].blockIndex = i;
        }
    }

    public void FruitsBlockDown()
    {
        GameObject.Find("FruitsCreateManager").GetComponent<FruitsCreateManager>().FruitsSelect(slotNum, this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        BlockInfoCreate();
        int preRandomColor = 0;
        slotFruitsList = new LinkedList<GameObject>();

        GameObject FruitsTmp = new GameObject();

        int allFruitsCount = 0;

        for(int i = 0; i < 3; i++)
        {
            allFruitsCount += JuiceGameInfoManager.Instance.fruitsCount[i];
        }

        if (allFruitsCount / 4 > 4)
        {
            allFruitsCount = 4;
        }

        for (int i = 0; i < allFruitsCount; i++, blockLength++)
        {
            if (0 == i)
            {
                preRandomColor = Random.Range((int)FRUITS_COLOR.FRUITS_RED, (int)FRUITS_COLOR.FRUITS_END);
                FruitsTmp = fruitsCreateMgr.GetComponent<FruitsCreateManager>().GetFruitsArr(preRandomColor);
                if(!FruitsTmp)
                {
                    break;
                }
                FruitsAddLast(FruitsTmp, blockInfo[0], preRandomColor);
                fruitsCreateMgr.GetComponent<FruitsCreateManager>().DegreaseFruitsIndex(preRandomColor);
                FruitsTmp.GetComponent<Fruits>().fruitsType = preRandomColor;
            }
            else
            {
                int nextRandomColor = NoOverlapFruitsType(preRandomColor);

                if (-1 == nextRandomColor)
                {
                    return;
                }

                FruitsTmp = fruitsCreateMgr.GetComponent<FruitsCreateManager>().GetFruitsArr(nextRandomColor);

                if (!FruitsTmp)
                {
                    break;
                }

                FruitsAddLast(FruitsTmp, blockInfo[i], nextRandomColor);
                fruitsCreateMgr.GetComponent<FruitsCreateManager>().DegreaseFruitsIndex(nextRandomColor);
                FruitsTmp.GetComponent<Fruits>().fruitsType = nextRandomColor;
                preRandomColor = nextRandomColor;
            }
        }

    }

    private void Update()
    {
        FruitsAddFirst();
    }

    public void FruitsSelect()
    {
        if (0 == slotFruitsList.Count)
        {
            return;
        }

        SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_FRUIT_MOVE);
        slotFruitsList.Last.Value.transform.localScale *= 1.2f;
        slotFruitsList.Last.Value.GetComponent<Fruits>().EnableShader(true);

        //slotFruitsList.Last.Value.GetComponent<Outline>().enabled = true;
    }

    public void FruitsUnSelect()
    {
        if (0 == slotFruitsList.Count)
        {
            return;
        }

        slotFruitsList.Last.Value.transform.localScale /= 1.2f;
        slotFruitsList.Last.Value.GetComponent<Fruits>().EnableShader(false);
        //slotFruitsList.Last.Value.GetComponent<Outline>().enabled = false;
    }

    void FruitsAddLast(GameObject _fruits, tagBlockInfo _blockInfo, int _fruitsType)
    {
        if (null == _fruits)
        {
            return;
        }

        _fruits.transform.SetParent(transform);
        _fruits.transform.position = _blockInfo.blockPosition;
        _fruits.transform.localScale = Vector2.one;
        _fruits.GetComponent<Fruits>().slotNum = slotNum;
        _fruits.GetComponent<Fruits>().fruitsType = _fruitsType;

        slotFruitsList.AddLast(_fruits);
    }

    public bool FruitsMove(LinkedList<GameObject> _fruitsList)
    {
        if (9 == slotFruitsList.Count)
        {
            return true;
        }

        SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_FRUIT_MOVE);
        _fruitsList.Last.Value.transform.SetParent(transform);
        _fruitsList.Last.Value.transform.position = blockInfo[slotFruitsList.Count].blockPosition;
        _fruitsList.Last.Value.GetComponent<Fruits>().slotNum = slotNum;

        slotFruitsList.AddLast(_fruitsList.Last.Value);

        return false;
    }

    public void SlotFruitsDelete()
    {
        slotFruitsList.RemoveLast();
    }

    public bool FruitsBomb()
    {
        if(3 > slotFruitsList.Count)
        {
            return false;
        }
        else
        {
            int fruitsTypeTmp = slotFruitsList.Last.Value.GetComponent<Fruits>().fruitsType;

            if (slotFruitsList.Last.Previous.Value.GetComponent<Fruits>().fruitsType == fruitsTypeTmp
                && slotFruitsList.Last.Previous.Previous.Value.GetComponent<Fruits>().fruitsType == fruitsTypeTmp)
            {
                for (int i = 0; i < 3; i++)
                {
                    FruitsDestroy();
                }
                SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_FRUIT_BURST);
                JuiceGameTextManager.Instance.OutputFruitsCount();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void FruitsDestroy()
    {
        slotFruitsList.Last.Value.GetComponent<Fruits>().Dead();
        slotFruitsList.RemoveLast();
    }

    public void FruitsAddFirst()
    {
        if (3 < slotFruitsList.Count || 0 == slotFruitsList.Count)
        {
            return;
        }

        int noOverlapNum = NoOverlapFruitsType(slotFruitsList.First.Value.GetComponent<Fruits>().fruitsType);

        if (-1 == noOverlapNum)
        {
            return;
        }

        for(int i = 0; i < 4 - slotFruitsList.Count; i++)
        {
            GameObject FruitsTmp = fruitsCreateMgr.GetComponent<FruitsCreateManager>().GetFruitsArr(noOverlapNum);

            if (!FruitsTmp)
            {
                break;
            }

            FruitsTmp.transform.SetParent(transform.GetChild(0).transform);
            FruitsTmp.transform.position = blockInfo[i].blockPosition;
            FruitsTmp.transform.localScale = Vector2.one;
            FruitsTmp.GetComponent<Fruits>().slotNum = slotNum;
            FruitsTmp.GetComponent<Fruits>().fruitsType = noOverlapNum;
            FruitsTmp.transform.SetAsFirstSibling();

            slotFruitsList.AddFirst(FruitsTmp);
            fruitsCreateMgr.GetComponent<FruitsCreateManager>().DegreaseFruitsIndex(noOverlapNum);
        }

        int j = slotFruitsList.Count - 1;
        for (LinkedListNode<GameObject> iter = slotFruitsList.Last; iter != slotFruitsList.First; iter = iter.Previous, j--)
        {
            iter.Value.transform.position = blockInfo[j].blockPosition;
        }
    }

    int NoOverlapFruitsType(int _fruitsType)
    {
        int[] fruitsTypeArrTmp = { 0, 0 };
        for (int i = 0, j = 0; i < 3; i++)
        {
            if (_fruitsType == i)
            {
                continue;
            }
            else
            {
                fruitsTypeArrTmp[j] = i;
                j++;
            }
        }

        return fruitsCreateMgr.GetComponent<FruitsCreateManager>().SearchMinFruitsType(fruitsTypeArrTmp[0], fruitsTypeArrTmp[1]);
    }

    public LinkedList<GameObject> FruitsList()
    {
        return slotFruitsList;
    }
}
