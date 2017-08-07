using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceContainer : MonoBehaviour {
    public int juiceType;
    public bool insertCheck;
    //public GameObject juice_b_insert_point;
    //bool upCheck;
    //float moveUpSpeed = 50.0f;
    //float juice_b_insert_positionY;
    //Vector3 juice_b_originalPos;
    //Transform parentTrans;

    public void ClickContainer()
    {
        if (0 >= JuiceGameInfoManager.Instance.juiceAmount[juiceType])
        {
            return;
        }

        SoundManager.Instance.PlayOneShot(SoundManager.SOUNDNUM.FX_JUICE_SELECT);
        if (JuiceGameInfoManager.Instance.mixCheck)
        {
            GameObject.Find("juice_m_top").GetComponent<Juice_m_top>().ClickJuiceContainerMixItemUse(juiceType);
        }
        else
        {
            GameObject.Find("juice_m_top").GetComponent<Juice_m_top>().ClickJuiceContainer(juiceType);
        }
    }

	// Use this for initialization
	void Start () {
        //parentTrans = transform.parent;
        //juice_b_insert_positionY = juice_b_insert_point.transform.position.y;
        //juice_b_originalPos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        
    }

    //public void SetMoveUp(bool _upCheck)
    //{
    //    upCheck = _upCheck;
    //}

    //public void SetInsertPosition()
    //{
    //    transform.position = new Vector3(transform.position.x, juice_b_insert_positionY);
    //}
}
