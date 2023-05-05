using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueProfile : MonoBehaviour
{
    // OnTrigger가 발동하면 delegate으로 상호작용 버튼 활성화 이미지 출력을 위한 본인의 Position을 UIDialogueManager에 전달함
    public delegate void OnTriggerEnterCallback(bool _isNpc, Transform _target);
    public delegate void OnTriggerEnterTargetCallback(DialogueProfile _profile);
    public delegate void OnTriggerExitCallback();
    public delegate void OnTriggerExitTargetCallback();

    private OnTriggerEnterCallback enterCallback = null;
    private OnTriggerEnterTargetCallback enterTargetCallback = null;
    private OnTriggerExitCallback exitCallback = null;
    private OnTriggerExitTargetCallback exitTargetCallback = null;

    [SerializeField]
    private string sid = string.Empty; // 사용할 대본의 ID
    [SerializeField]
    private int tid = 0; // 대사의 주인인지 확인을 위한 ID
    [SerializeField]
    private bool isNpc = false; // NPC인지 여부 (미확정)

    private bool isAction = false; // 상호작용 중인지 여부
    private bool isDialOver = false; // 대화가 한 사이클이 돌았는지 여부


    #region Get/Set
    public string GetSid()
    {
        return sid;
    }

    public int GetTid()
    {
        return tid;
    }

    public bool IsNPC()
    {
        return isNpc;
    }

    public bool IsAction()
    {
        return isAction;
    }
    public void IsAction(bool _action)
    {
        isAction = _action;
    }
    public bool IsDialOver()
    {
        return isDialOver;
    }
    public void IsDialOver(bool _over)
    {
        isDialOver = _over;
    }
    #endregion


    public void Init(
        OnTriggerEnterCallback _enterCallback, // Enter
        OnTriggerEnterTargetCallback _targetCallback, // Enter
        OnTriggerExitCallback _exitCallback, // Exit
        OnTriggerExitTargetCallback _exitTargetCallback // Exit
        )
    {
#if UNITY_EDITOR
        //Debug.Log("[DialogueProfile] Init");
#endif
        enterCallback = _enterCallback;
        enterTargetCallback = _targetCallback;
        exitCallback = _exitCallback;
        exitTargetCallback = _exitTargetCallback;
    }


    /// Callback ///
    #region Callbacks
    public void OnTriggerEnter(Collider _other)
    {
        if(_other.CompareTag("Player"))
        {
#if UNITY_EDITOR
            //Debug.LogFormat("[Player - OnTriggerEnter] callback: {0}, targetCallback: {1}", enterCallback.Method.Name, enterTargetCallback.Method.Name);
#endif
            enterCallback?.Invoke(isNpc, transform);
            enterTargetCallback?.Invoke(this);
        }
    }

    public void OnTriggerExit(Collider _other)
    {
        if(_other.CompareTag("Player"))
        {
#if UNITY_EDITOR
            //Debug.LogFormat("[Player - OnTriggerExit] callback: {0}, targetCallback: {1}", exitCallback.Method.Name, exitTargetCallback.Method.Name);
#endif
            exitCallback?.Invoke();
            exitTargetCallback?.Invoke();
        }
    }
    #endregion
}
