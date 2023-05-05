using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Newtonsoft.Json;

public class DialogueProcess : MonoBehaviour
{
    //수정안
    // DialogueProcess에서 대본을 처리함
    // Process는 대본을 처리하고 delegate로 적절한 곳에 데이터를 실어서 보냄

    // 대본 처리 영역
    // DialogueProcess는 분류가 끝나면 대사를 UIDialogueManager에 보냄
    // UIDialogueManager => 대사 처리 (현재 대사 밖에 없음)
    // UIDialogueManager에서는 Coroutine을 통해 대사를 순차적으로 출력함
    public delegate void OnStartDialogue(int _tid, List<SDialogue> _scripts);
    public delegate bool IsDialogueOverCallback(); // NPC와의 대본을 끝마쳤는지 여부 확인
    private OnStartDialogue onStartDialogue = null;
    private IsDialogueOverCallback isDialogueOver = null;

    // 현재 '일반 대화' 기능만 있음
    // Conversation: 일반 대화
    // Quest: 퀘스트 수락여부 상호작용을 적용해서 출력할 수 있을 예정
    public enum EDialogueType { Conversation, Quest };

    [Serializable]
    public struct SDialogue
    {
        [JsonProperty("idx")]
        public int idx; // 대본의 순서 index
        [JsonProperty("tid")]
        public int tid; // 대사를 하는 NPC의 부여된 ID
        [JsonProperty("type")]
        public EDialogueType type; // 대사의 타입
        [JsonProperty("script")]
        public string script; // 대사 내용
    }
    [Serializable]
    public struct SSceneDialogue
    {
        [JsonProperty("sid")]
        public string sid;
        [JsonProperty("scripts")]
        public List<SDialogue> scripts; // 대사들
        [JsonProperty("repeated")]
        public List<SDialogue> repeated; // 대화 종료 후 반복 대사
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


    // Dialogue가 시작되면 파일로부터 데이터를 읽어들임
    private List<SDialogue> GetScriptData(string _sid)
    {
        // List 초기화
        listScript = null;

        // 임시 데이터를 활용해서 테스트 (a00, a01)
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
            if (listSceneDialogue[i].sid == _sid) // 해당하는 Scene대본이라면 List에 담음
            {
                if ((bool)isDialogueOver?.Invoke() == true) // true이면 대본을 끝마친 상태이므로 반복 대사로 변경
                    return listSceneDialogue[i].repeated;
                else
                    return listSceneDialogue[i].scripts;
            }
        }

        return null; // 해당 대본이 존재하지 않으면 null 반환

    }

    // Scene 대본을 가져오기 위한 sid를 받음
    public void StartDialogueProcess(string _sid, int _tid)
    {
        listScript = GetScriptData(_sid);

        if (listScript == null) return; // 받은 대본이 없다면 대화를 진행하지 않음

        onStartDialogue?.Invoke(_tid, listScript); // UIDialogue에 대사 전달
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
