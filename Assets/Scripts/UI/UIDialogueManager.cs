using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

using Newtonsoft.Json;

public class UIDialogueManager : MonoBehaviour
{
    public delegate void OnDialogueStartCallback(bool _bool); // ��ȭ�� �����ϸ� �ش� NPC�� Action���°� True�� �ǵ��� �ϱ� ���� Callback
    public delegate void OnDialogueOverCallback(bool _over); // �ش� NPC�� ��ȭ�� �����ƴ��� ���θ� �����ϱ� ���� Callback
    public delegate void OnDialogueEndCallback(); // ��ȭ�� �����ٸ� ���� ������ �̵��ϱ� ���� ��Ż ���� Callback
    private OnDialogueStartCallback onDialogueCallback = null;
    private OnDialogueOverCallback onDialogueOverCallback = null;
    private OnDialogueEndCallback onDialogueEndCallback = null;

    [SerializeField]
    private RectTransform canvasRt = null;
    
    private RectTransform dialRt = null; // ��ǳ��
    private RectTransform infoRt = null; // ��ȣ�ۿ� �ȳ� ����

    private TextMeshProUGUI dialText = null; // ��ǳ�� ��� ���
    private float speed = 1f; // ��� ��� �ӵ� ����

    private string sid = string.Empty; // �뺻 ���̵� Ȯ��
    private int tid = 0; // �뺻������ NPC ���̵�
    private List<DialogueProcess.SDialogue> listScripts = null; // �뺻 ����Ʈ

    private bool isSkip = false; // ��ŵ�ϴ��� ���� Ȯ��
    private bool isScriptEnd = false; // �� ��簡 �������� Ȯ��
    private bool isDialEnd = false; // �뺻�� �������� Ȯ��


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
            // ������ DialogueCanvas�� Inspector�� ����� �Ǿ����� �ʴٸ� ����
            GameObject prefab = Resources.Load("Prefabs/UI/P_UI_DialogueCanvas_WorldPos") as GameObject;
            GameObject go = Instantiate(prefab);
            canvasRt = go.GetComponent<RectTransform>();
        }
        Transform dialTr = canvasRt.Find("DialogueGroup");
        Transform inputTr = canvasRt.Find("InputGroup");

        // ��ǳ��
        if (dialTr.TryGetComponent<RectTransform>(out dialRt))
        {
            SetActiveDialogueUI(false);
            dialText = dialRt.GetComponentInChildren<TextMeshProUGUI>();
        }
        // ��ȣ�ۿ� �ȳ� ����
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
        SetActiveDialogueUI(false); // ��ǳ�� - ��Ȱ��ȭ
        SetActiveInfoUI(false); // ��ȣ�ۿ� �ȳ� ���� - ��Ȱ��ȭ
        dialText.text = string.Empty; // ��ǳ�� �ؽ�Ʈ ���

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


    // OnTriggerEnter�̸� �ߵ��Ͽ� ��ȣ�ۿ��� ������ NPC�� ��ġ�� Canvas�� ��ġ�� �ű��
    // ��ȣ�ۿ� �ȳ� ������ Ȱ��ȭ ��Ŵ
    public void MoveUIToTarget(bool _isNpc, Transform _npcTr)
    {
        canvasRt.position = _npcTr.position; // UI�� NPC�� ��ġ�� �̵�

        if(_isNpc) // NPC�̸� ��ȣ�ۿ� Ȱ��ȭ
            SetActiveInfoUI(true); // ��ȣ�ۿ� �ȳ� ���� Ȱ��ȭ
    }

    // OnTriggerEnter ���¿��� �÷��̾ ��ȣ�ۿ�(isAction)�� �����ϸ�(->true) ��ǳ���� Ȱ��ȭ�ǰ�
    // DialogueManager�κ��� �뺻�� NPC�� ������ �޾Ƽ� �뺻�� �����
    public void StartDialogue(int _tid, List<DialogueProcess.SDialogue> _scripts)
    {
        if (_scripts == null || _scripts.Count == 0) return;
        

        SetActiveDialogueUI(true); // ��ǳ�� - ��Ȱ��ȭ
        SetActiveInfoUI(true); // ��ȣ�ۿ� �ȳ� ���� - ��Ȱ��ȭ

        IsSkip(false);
        IsScriptEnd(false);

        tid = _tid; // �뺻 ���� ������
        listScripts = _scripts;
        
        onDialogueCallback?.Invoke(true); // DialogueProfile�� isAction�� True�� ����
        StartPrintScriptQueue(listScripts);
    }

    // OnTriggerExit�̸� �ߵ��Ͽ� ��ǳ���� ��ȣ�ۿ� �ȳ� ���� ��Ȱ��ȭ
    public void LostUITarget()
    {
        StopPrintScript();
        StopScriptQueue();
        IsSkip(false);
        SetActiveDialogueUI(false);
        SetActiveInfoUI(false);
        onDialogueCallback?.Invoke(false);
    }

    // ��ŵ��ư ���� (Player�� delegate�� �Ѱ��� �Լ�)
    public void SkipScript()
    {
        isSkip = true;
    }

    // ��ǳ�� Ȱ��ȭ ����
    private void SetActiveDialogueUI(bool _active)
    {
        dialRt.gameObject.SetActive(_active);
    }
    // �ȳ�����
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
    // �뺻�� �����ͼ� Coroutine���� ����ϵ��� ��.
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
        while(i < _listDial.Count) // ���� �뺻�� ���� ������ �ݺ�
        {
            string script = _listDial[i].script;
#if UNITY_EDITOR
            //Debug.LogFormat("[QueueCoroutine] ���� script: {0}", script);
#endif
            StartPrintScript(script);
            yield return new WaitUntil(() => isSkip == true && isScriptEnd == true);
            StopPrintScript();
            ++i;
            yield return null;
        }

        IsDialogueEnd(true); // �뺻�� ����ħ

        if(isDialEnd)
        {
            SetActiveDialogueUI(false);
            onDialogueCallback?.Invoke(false); // �뺻�� �������Ƿ� NPC�� Action ���¸� false�� �ٲ�
            onDialogueOverCallback?.Invoke(true); // �뺻�� ���������Ƿ� NPC�� �뺻 ����ħ ���¸� true�� �ٲ�
            if (onDialogueEndCallback != null)
                onDialogueEndCallback?.Invoke(); // �뺻�� �����ƴٸ� �̵��� ���� ��Ż�� ��
        }
    }
    private IEnumerator PrintScriptCoroutine(string _script)
    {
        StringBuilder sb = new StringBuilder();
        dialText.text = string.Empty;
        IsScriptEnd(false);
        IsSkip(false);
#if UNITY_EDITOR
        //Debug.LogFormat("[ScriptCoroutine] �ؽ�Ʈ�� ����մϴ�: {0}", _script);
#endif

        int len = 0;
        while (len < _script.Length)
        {
            sb.Append(_script[len]);
            dialText.text = sb.ToString();
            ++len;

            if (isSkip == false) // ��ŵ���� �ʾҴٸ�
            {
                // Typing Sound
                if(_script[len - 1].ToString() != " ")
                    PlayDialogueTextSound();
                yield return new WaitForSeconds(0.15f * speed);
            }
            else // ��ŵ�ߴٸ�
            {
                //// Typing Sound
                PlayDialogueTextSound();
                break;
            }
        }

        // ��� ����� �������Ƿ� ���� ���°� �ǵ��� ����
        dialText.text = _script;
        IsScriptEnd(true); // ��簡 �������� ����
        IsSkip(false); // skip ���� �ʱ�ȭ
#if UNITY_EDITOR
        //Debug.LogFormat("[ScriptCoroutine] ����");
#endif
    }
#endregion
}
