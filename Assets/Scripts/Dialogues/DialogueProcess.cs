using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Newtonsoft.Json;

public class DialogueProcess : MonoBehaviour
{
    //������
    // DialogueProcess���� �뺻�� ó����
    // Process�� �뺻�� ó���ϰ� delegate�� ������ ���� �����͸� �Ǿ ����

    // �뺻 ó�� ����
    // DialogueProcess�� �з��� ������ ��縦 UIDialogueManager�� ����
    // UIDialogueManager => ��� ó�� (���� ��� �ۿ� ����)
    // UIDialogueManager������ Coroutine�� ���� ��縦 ���������� �����
    public delegate void OnStartDialogue(int _tid, List<SDialogue> _scripts);
    public delegate bool IsDialogueOverCallback(); // NPC���� �뺻�� �����ƴ��� ���� Ȯ��
    private OnStartDialogue onStartDialogue = null;
    private IsDialogueOverCallback isDialogueOver = null;

    // ���� '�Ϲ� ��ȭ' ��ɸ� ����
    // Conversation: �Ϲ� ��ȭ
    // Quest: ����Ʈ �������� ��ȣ�ۿ��� �����ؼ� ����� �� ���� ����
    public enum EDialogueType { Conversation, Quest };

    [Serializable]
    public struct SDialogue
    {
        [JsonProperty("idx")]
        public int idx; // �뺻�� ���� index
        [JsonProperty("tid")]
        public int tid; // ��縦 �ϴ� NPC�� �ο��� ID
        [JsonProperty("type")]
        public EDialogueType type; // ����� Ÿ��
        [JsonProperty("script")]
        public string script; // ��� ����
    }
    [Serializable]
    public struct SSceneDialogue
    {
        [JsonProperty("sid")]
        public string sid;
        [JsonProperty("scripts")]
        public List<SDialogue> scripts; // ����
        [JsonProperty("repeated")]
        public List<SDialogue> repeated; // ��ȭ ���� �� �ݺ� ���
    }

    List<SDialogue> listScript = null;


    private void Start()
    {
        // For Test
        //GetScriptData("prologue01");
    }


    public void Init(OnStartDialogue _startDialogue, IsDialogueOverCallback _isDialogueOver)
    {
#if UNITY_EDITOR
        //Debug.Log("[DialogueProcess] Init");
#endif
        onStartDialogue = _startDialogue;
        isDialogueOver = _isDialogueOver;
    }


    // Dialogue�� ���۵Ǹ� ���Ϸκ��� �����͸� �о����
    private List<SDialogue> GetScriptData(string _sid)
    {
        // List �ʱ�ȭ
        listScript = null;

        // �ӽ� �����͸� Ȱ���ؼ� �׽�Ʈ (a00, a01)
        FileManager fileManager = new FileManager();
        string[] split = Regex.Split(_sid, @"[0-9]");
        string did = split[0]; // sid = a10 -> did = a
        string json = fileManager.GetScriptData(did);

        List<SSceneDialogue> listSceneDialogue = JsonConvert.DeserializeObject<List<SSceneDialogue>>(json);
#if UNITY_EDITOR
        //Debug.LogFormat("Dialogue Data: {0}", json);
        //PrintList(listSceneDialogue);
#endif
        for (int i = 0; i < listSceneDialogue.Count; ++i)
        {
            if (listSceneDialogue[i].sid == _sid) // �ش��ϴ� Scene�뺻�̶�� List�� ����
            {
                if ((bool)isDialogueOver?.Invoke() == true) // true�̸� �뺻�� ����ģ �����̹Ƿ� �ݺ� ���� ����
                    return listSceneDialogue[i].repeated;
                else
                    return listSceneDialogue[i].scripts;
            }
        }

        return null; // �ش� �뺻�� �������� ������ null ��ȯ

    }

    // Scene �뺻�� �������� ���� sid�� ����
    public void StartDialogueProcess(string _sid, int _tid)
    {
        listScript = GetScriptData(_sid);

        if (listScript == null) return; // ���� �뺻�� ���ٸ� ��ȭ�� �������� ����

        onStartDialogue?.Invoke(_tid, listScript); // UIDialogue�� ��� ����
    }


    /// Print ///
    #region Print Scripts
    private void PrintDictionary(Dictionary<string, List<SDialogue>> _dic)
    {
#if UNITY_EDITOR
        List<string> keys = new List<string>(_dic.Keys);
        for(int i = 0; i < keys.Count; ++i)
        {
            Debug.LogFormat("sid: {0}", keys[i]);
            List<SDialogue> list = _dic[keys[i]];
            for(int j = 0; j < list.Count; ++j)
            {
                Debug.LogFormat("idx: {0}, tid: {1}, type: {2}, script: {3}", list[j].idx, list[j].tid, list[j].type, list[j].script);
            }
        }
#endif
    }

    private void PrintList(List<SSceneDialogue> _list)
    {
#if UNITY_EDITOR
        for(int i = 0; i < _list.Count; ++i)
        {
            Debug.LogFormat("sid: {0}, scripts: {1}", _list[i].sid, _list[i].scripts);
            PrintList2(_list[i].scripts);
        }
#endif
    }
    private void PrintList2(List<SDialogue> _list)
    {
#if UNITY_EDITOR
        for(int i = 0; i< _list.Count; ++i)
        {
            Debug.LogFormat("idx: {0}, tid: {1}, type: {2}, script: {3}", _list[i].idx, _list[i].tid, _list[i].type.ToString(), _list[i].script);
        }
#endif
    }
    #endregion


    
}
