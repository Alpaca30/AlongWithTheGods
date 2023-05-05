using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    public delegate void SkipCallback();            // 대화 스킵
    public delegate bool IsActionCallback();        // 대화 중인지 여부
    public delegate void OnDialogueCallback();      // 대화 시작
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
        // 대화
        if(Input.GetKeyDown(KeyCode.V))
        {
            if (actionCallback == null) return;

            // Action(대화)중이라면 Skip 기능을. 아니라면 상호작용 하는 기능으로
            if((bool)actionCallback?.Invoke())
            {
                // 대화 Skip
                skipCallback?.Invoke();
            }
            else
            {
                // 대화 상호작용
                dialCallback?.Invoke();
            }
        }
    }
}
