using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueProfile : MonoBehaviour
{
    // OnTrigger�� �ߵ��ϸ� delegate���� ��ȣ�ۿ� ��ư Ȱ��ȭ �̹��� ����� ���� ������ Position�� UIDialogueManager�� ������
    public delegate void OnTriggerEnterCallback(bool _isNpc, Transform _target);
    public delegate void OnTriggerEnterTargetCallback(DialogueProfile _profile);
    public delegate void OnTriggerExitCallback();
    public delegate void OnTriggerExitTargetCallback();

    private OnTriggerEnterCallback enterCallback = null;
    private OnTriggerEnterTargetCallback enterTargetCallback = null;
    private OnTriggerExitCallback exitCallback = null;
    private OnTriggerExitTargetCallback exitTargetCallback = null;

    [SerializeField]
    private string sid = string.Empty; // ����� �뺻�� ID
    [SerializeField]
    private int tid = 0; // ����� �������� Ȯ���� ���� ID
    [SerializeField]
    private bool isNpc = false; // NPC���� ���� (��Ȯ��)

    private bool isAction = false; // ��ȣ�ۿ� ������ ����
    private bool isDialOver = false; // ��ȭ�� �� ����Ŭ�� ���Ҵ��� ����


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
