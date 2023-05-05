using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    public delegate void SkipCallback();            // ��ȭ ��ŵ
    public delegate bool IsActionCallback();        // ��ȭ ������ ����
    public delegate void OnDialogueCallback();      // ��ȭ ����
    private SkipCallback skipCallback = null;
    private IsActionCallback actionCallback = null;
    private OnDialogueCallback dialCallback = null;


    private void Update()
    {
        OnClickSkipTalk();
    }


    public void Init(SkipCallback _skipCallback, IsActionCallback _actionCallback, OnDialogueCallback _dialCallback)
    {
        skipCallback = _skipCallback;
        actionCallback = _actionCallback;
        dialCallback = _dialCallback;
    }


    private void OnClickSkipTalk()
    {
        // ��ȭ
        if(Input.GetKeyDown(KeyCode.V))
        {
            if (actionCallback == null) return;

            // Action(��ȭ)���̶�� Skip �����. �ƴ϶�� ��ȣ�ۿ� �ϴ� �������
            if((bool)actionCallback?.Invoke())
            {
                // ��ȭ Skip
                skipCallback?.Invoke();
            }
            else
            {
                // ��ȭ ��ȣ�ۿ�
                dialCallback?.Invoke();
            }
        }
    }
}
