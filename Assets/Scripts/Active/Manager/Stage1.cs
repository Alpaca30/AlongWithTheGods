using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    public delegate void Stage1Delegate();
    private Stage1Delegate stage1Callback = null;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            stage1Callback?.Invoke();
            gameObject.SetActive(false);
        }
    }
    public void SetStage1Delegate(Stage1Delegate _stage1Callback)
    {
        stage1Callback = _stage1Callback;
    }
}
