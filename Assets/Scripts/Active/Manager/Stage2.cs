using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : MonoBehaviour
{
    public delegate void Stage2Delegate();
    private Stage2Delegate stage2Callback = null;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            stage2Callback?.Invoke();
            gameObject.SetActive(false);
        }
    }
    public void SetStage2Delegate(Stage2Delegate _stage2Callback)
    {
        stage2Callback = _stage2Callback;
    }
}
