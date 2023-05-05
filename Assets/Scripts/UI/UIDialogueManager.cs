using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

using Newtonsoft.Json;

public class UIDialogueManager : MonoBehaviour
{
    public delegate void OnDialogueStartCallback(bool _bool); // 대화를 시작하면 해당 NPC가 Action상태가 True가 되도록 하기 위한 Callback
    public delegate void OnDialogueOverCallback(bool _over); // 해당 NPC와 대화를 끝마쳤는지 여부를 설정하기 위한 Callback
    public delegate void OnDialogueEndCallback(); // 대화가 끝났다면 현재 씬에서 이동하기 위한 포탈 열기 Callback
    private OnDialogueStartCallback onDialogueCallback = null;
    private OnDialogueOverCallback onDialogueOverCallback = null;
    private OnDialogueEndCallback onDialogueEndCallback = null;

    [SerializeField]
    private RectTransform canvasRt = null;
    
    private RectTransform dialRt = null; // 말풍선
    private RectTransform infoRt = null; // 상호작용 안내 문구

    private TextMeshProUGUI dialText = null; // 말풍선 대사 출력
    private float speed = 1f; // 대사 출력 속도 조절

    private string sid = string.Empty; // 대본 아이디 확인
    private int tid = 0; // 대본에서의 NPC 아이디
    private List<DialogueProcess.SDialogue> listScripts = null; // 대본 리스트

    private bool isSkip = false; // 스킵하는지 여부 확인
    private bool isScriptEnd = false; // 한 대사가 끝났는지 확인
    private bool isDialEnd = false; // 대본이 끝났는지 확인


    // Get, Set
    private void IsSkip(bool _skip)
    {
        isSkip = _skip;
    }
    private void IsScriptEnd(bool _end)
    {
        isScriptEnd = _end;
    }
    private void IsDialogueEnd(bool _end)
    {
        isDialEnd = _end;
    }


    private void Awake()
    {
        if (canvasRt == null)
        {
            // 조작할 DialogueCanvas가 Inspector에 등록이 되어있지 않다면 생성
            GameObject prefab = Resources.Load("Prefabs/UI/P_UI_DialogueCanvas_WorldPos") as GameObject;
            GameObject go = Instantiate(prefab);
            canvasRt = go.GetComponent<RectTransform>();
        }
        Transform dialTr = canvasRt.Find("DialogueGroup");
        Transform inputTr = canvasRt.Find("InputGroup");

        // 말풍선
        if (dialTr.TryGetComponent<RectTransform>(out dialRt))
        {
            SetActiveDialogueUI(false);
            dialText = dialRt.GetComponentInChildren<TextMeshProUGUI>();
        }
        // 상호작용 안내 문구
        if(inputTr.TryGetComponent<RectTransform>(out infoRt))
        {
            SetActiveInfoUI(false);
        }
    }


    public void Init(OnDialogueStartCallback _dialCallback, OnDialogueOverCallback _dialOverCallback, OnDialogueEndCallback _dialEndCallback)
    {
#if UNITY_EDITOR
        //Debug.Log("[UIDialogueManager] Init");
#endif
        // Coroutine
        StopCoroutine("PrintScriptCoroutine");
        StopCoroutine("PrintScriptQueueCoroutine");

        // UI
        SetActiveDialogueUI(false); // 말풍선 - 비활성화
        SetActiveInfoUI(false); // 상호작용 안내 문구 - 비활성화
        dialText.text = string.Empty; // 말풍선 텍스트 비움

        // Script Info
        sid = string.Empty;
        tid = 0;
        listScripts = null;

        IsSkip(false);
        IsScriptEnd(false);
        IsDialogueEnd(false);

        onDialogueCallback = _dialCallback;
        onDialogueOverCallback = _dialOverCallback;
        onDialogueEndCallback = _dialEndCallback;
    }


    // OnTriggerEnter이면 발동하여 상호작용이 가능한 NPC의 위치로 Canvas의 위치를 옮기고
    // 상호작용 안내 문구를 활성화 시킴
    public void MoveUIToTarget(bool _isNpc, Transform _npcTr)
    {
        canvasRt.position = _npcTr.position; // UI를 NPC의 위치로 이동

        if(_isNpc) // NPC이면 상호작용 활성화
            SetActiveInfoUI(true); // 상호작용 안내 문구 활성화
    }

    // OnTriggerEnter 상태에서 플레이어가 상호작용(isAction)을 시작하면(->true) 말풍선이 활성화되고
    // DialogueManager로부터 대본과 NPC의 정보를 받아서 대본을 출력함
    public void StartDialogue(int _tid, List<DialogueProcess.SDialogue> _scripts)
    {
        if (_scripts == null || _scripts.Count == 0) return;
        

        SetActiveDialogueUI(true); // 말풍선 - 비활성화
        SetActiveInfoUI(true); // 상호작용 안내 문구 - 비활성화

        IsSkip(false);
        IsScriptEnd(false);

        tid = _tid; // 대본 주인 구분자
        listScripts = _scripts;
        
        onDialogueCallback?.Invoke(true); // DialogueProfile의 isAction을 True로 변경
        StartPrintScriptQueue(listScripts);
    }

    // OnTriggerExit이면 발동하여 말풍선과 상호작용 안내 문구 비활성화
    public void LostUITarget()
    {
        StopPrintScript();
        StopScriptQueue();
        IsSkip(false);
        SetActiveDialogueUI(false);
        SetActiveInfoUI(false);
        onDialogueCallback?.Invoke(false);
    }

    // 스킵버튼 동작 (Player의 delegate에 넘겨줄 함수)
    public void SkipScript()
    {
        isSkip = true;
    }

    // 말풍선 활성화 여부
    private void SetActiveDialogueUI(bool _active)
    {
        dialRt.gameObject.SetActive(_active);
    }
    // 안내문구
    private void SetActiveInfoUI(bool _active)
    {
        infoRt.gameObject.SetActive(_active);
    }

    private void PlayDialogueTextSound()
    {
        SoundManager.Instance.Play("DialogueText");
    }

    /// Coroutine ///
    #region Coroutins
    private void StartPrintScriptQueue(List<DialogueProcess.SDialogue> _listDial)
    {
        if (_listDial.Count == 0 || _listDial == null) return;

        StartCoroutine("PrintScriptQueueCoroutine", _listDial);
    }
    // 대본을 가져와서 Coroutine으로 출력하도록 함.
    private void StartPrintScript(string _script)
    {
        if (_script == "" || _script == null || _script == string.Empty) return;

        StartCoroutine("PrintScriptCoroutine", _script);
    }

    private void StopScriptQueue()
    {
        StopCoroutine("PrintScriptQueueCoroutine");
    }
    private void StopPrintScript()
    {
        StopCoroutine("PrintScriptCoroutine");
    }

    private IEnumerator PrintScriptQueueCoroutine(List<DialogueProcess.SDialogue> _listDial)
    {

        int i = 0;
        while(i < _listDial.Count) // 다음 대본이 없을 때까지 반복
        {
            string script = _listDial[i].script;
#if UNITY_EDITOR
            //Debug.LogFormat("[QueueCoroutine] 다음 script: {0}", script);
#endif
            StartPrintScript(script);
            yield return new WaitUntil(() => isSkip == true && isScriptEnd == true);
            StopPrintScript();
            ++i;
            yield return null;
        }

        IsDialogueEnd(true); // 대본을 끝마침

        if(isDialEnd)
        {
            SetActiveDialogueUI(false);
            onDialogueCallback?.Invoke(false); // 대본이 끝났으므로 NPC의 Action 상태를 false로 바꿈
            onDialogueOverCallback?.Invoke(true); // 대본을 끝마쳤으므로 NPC의 대본 끝마침 상태를 true로 바꿈
            if (onDialogueEndCallback != null)
                onDialogueEndCallback?.Invoke(); // 대본을 끝마쳤다면 이동을 위한 포탈을 염
        }
    }
    private IEnumerator PrintScriptCoroutine(string _script)
    {
        StringBuilder sb = new StringBuilder();
        dialText.text = string.Empty;
        IsScriptEnd(false);
        IsSkip(false);
#if UNITY_EDITOR
        //Debug.LogFormat("[ScriptCoroutine] 텍스트를 출력합니다: {0}", _script);
#endif

        int len = 0;
        while (len < _script.Length)
        {
            sb.Append(_script[len]);
            dialText.text = sb.ToString();
            ++len;

            if (isSkip == false) // 스킵하지 않았다면
            {
                // Typing Sound
                if(_script[len - 1].ToString() != " ")
                    PlayDialogueTextSound();
                yield return new WaitForSeconds(0.15f * speed);
            }
            else // 스킵했다면
            {
                //// Typing Sound
                PlayDialogueTextSound();
                break;
            }
        }

        // 대사 출력이 끝났으므로 최종 상태가 되도록 설정
        dialText.text = _script;
        IsScriptEnd(true); // 대사가 끝났는지 여부
        IsSkip(false); // skip 여부 초기화
#if UNITY_EDITOR
        //Debug.LogFormat("[ScriptCoroutine] 종료");
#endif
    }
#endregion
}
