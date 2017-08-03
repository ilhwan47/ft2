using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopChar : MonoBehaviour
{
    string str = "안녕하세요. 반갑습니다. \n이곳은 아이템을 살 수 있는 상점이에요. \n터치해서 아이템 리스트를 봅시다.";

    public GameObject _goChar;
    public GameObject _goTextBox;
    public UILabel _labelText;

    Coroutine _co = null;

    public void Open()
    {
        _co = StartCoroutine(IEOpen());
    }

    public void Close()
    {
        if (null != _co)
        {
            iTween.Stop();
            StopCoroutine(_co);
        }
    }

    IEnumerator IEOpen()
    {
        _goChar.SetActive(true);
        _goChar.transform.localPosition = new Vector3(-800f, _goChar.transform.localPosition.y, _goChar.transform.localPosition.z);
        iTween.MoveTo(_goChar, iTween.Hash("x", 120.0f, "islocal", true, "easeType", iTween.EaseType.easeOutQuart, "delay", 0.1f, "time", 0.4f));
        _goTextBox.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        _goTextBox.SetActive(true);
        _labelText.text = "";
        Vector3 vec = _goTextBox.transform.localScale;
        _goTextBox.transform.localScale = Vector3.zero;
        iTween.ScaleTo(_goTextBox, iTween.Hash("x", vec.x, "y", vec.y, "z", vec.z, "easeType", iTween.EaseType.easeOutBack, "time", 0.5f));
        yield return new WaitForSeconds(0.5f);

        _labelText.text = "";
        for (int i=0; i<=str.Length; i++)
        {
            _labelText.text = str.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }

        yield break;
    }
}
