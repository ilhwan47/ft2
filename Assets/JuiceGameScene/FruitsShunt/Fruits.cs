using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fruits : MonoBehaviour {
    public int slotNum;
    public int blockNum;
    public int fruitsType;

    public bool shaderEnable;

    GameObject Juice_m_top;
    Material outlineShader;

	// Use this for initialization
	void Start () {
        outlineShader = gameObject.GetComponent<Image>().material;
        gameObject.GetComponent<Image>().material = null;
        Juice_m_top = GameObject.Find("juice_m_top");
        outlineShader.SetShaderPassEnabled("Sprite/SpriteOutline", false);
    }

    // Update is called once per frame
    void Update () {

    }

    public void EnableShader(bool _check)
    {
        if (!_check)
        {
            gameObject.GetComponent<Image>().material = null;
        }
        else
        {
            gameObject.GetComponent<Image>().material = outlineShader;
        }
    }

    public void Dead()
    {
        JuiceGameInfoManager.Instance.MinusFruitsCount(fruitsType);
        Juice_m_top.GetComponent<Juice_m_top>().AddJuice(fruitsType);
        Destroy(this.gameObject);
    }

}
