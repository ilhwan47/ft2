using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JuiceGameButtonManager : MonoBehaviour {
    public GameObject panel_tmp;
	// Use this for initialization
    public void ClickGameExitBtn()
    {
        Time.timeScale = 0;
        panel_tmp.SetActive(true);
    }

    //public void ClickGameE
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }
}
