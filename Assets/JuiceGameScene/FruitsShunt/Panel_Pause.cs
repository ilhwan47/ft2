using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Pause : MonoBehaviour {
    public Text coolCountText;
    public Text badCountText;
    public Text comboCountText;

    public Text incPopText;
    public Text decPopText;
    public Text maxComboText;

    Vector3 prePosition;// = new Vector3();

    // Use this for initialization
    private void Awake()
    {
        prePosition = transform.position;
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Init();
    }

    public void ClickReturnGame()
    {
        Time.timeScale = 1;

        SetDisable();
    }

    void Init()
    {
        transform.position = transform.parent.position;
        transform.localScale = new Vector3(1f, 1f, 1f);
        coolCountText.text = JuiceGameInfoManager.Instance.coolCount.ToString();
        badCountText.text = JuiceGameInfoManager.Instance.badCount.ToString();
        comboCountText.text = JuiceGameInfoManager.Instance.combo.ToString();
        incPopText.text = JuiceGameInfoManager.Instance.incPopCount.ToString();
        decPopText.text = JuiceGameInfoManager.Instance.decPopCount.ToString();
        maxComboText.text = JuiceGameInfoManager.Instance.maxCombo.ToString();
    }

    void SetDisable()
    {
        transform.position = prePosition;
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.SetActive(false);
    }
}
