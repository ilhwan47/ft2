using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopPopup : MonoBehaviour
{
    public GameObject _goPopup;

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
        _goPopup.SetActive(true);
        Vector3 vec = _goPopup.transform.localScale;
        _goPopup.transform.localScale = Vector3.zero;
        iTween.ScaleTo(_goPopup, iTween.Hash("x", vec.x, "y", vec.y, "z", vec.z, "easeType", iTween.EaseType.easeOutBack, "time", 0.5f));
        yield return new WaitForSeconds(0.5f);
        
        yield break;
    }
}
