using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3: MonoBehaviour
{
    public delegate void Stage3Delegate();
    private Stage3Delegate stage3Callback = null;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            stage3Callback?.Invoke();
            gameObject.SetActive(false);
        }
    }
    public void SetStage3Delegate(Stage3Delegate _stage3Callback)
    {
        stage3Callback = _stage3Callback;
    }
}
