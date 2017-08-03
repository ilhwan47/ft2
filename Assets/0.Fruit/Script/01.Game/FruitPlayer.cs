using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPlayer : MonoBehaviour
{
    FruitGameManager.delOpenUI _del = null;

    public void SetDelegate(FruitGameManager.delOpenUI del)
    {
        _del = del;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Fruit._this.OpenUI(collision.gameObject.name);

        if (null != _del)
            _del(collision.gameObject);
    }
}
