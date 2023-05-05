using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    /*
    1) 플레이어가 NPC 간 OnTriggerEnter가 발동하면 상호작용 문구를 활성화 시킴
    1-a) 플레이어와 NPC 간의 대화 상호작용 발동 (임시 상호작용 버튼: Space Bar)
    2) 상호작용하는 NPC의 정보를 가져와서 저장
    3) NPC의 정보를 통해 대화내용을 FileManager에서 읽어오도록 함
    4) 불러온 대본을 DialogueProcess에서 Json형태로 Deserialize를 해서 구조체에 저장함
    5) Json형태로 저장한 대본 정보를 DialogueManager로 가져옴
    6) DialogueManager에서 UIDialogueManager로 대본 정보와 NPC의 정보를 넘김
    7) UIDialogueManager에서는 숨겨둔 말풍선 위치를 넘겨받은 Position으로 이동시킴
    8) 이동 후 숨겨둔 말풍선을 보이게 만듦
    9) 대본을 Coroutine을 이용하여 출력하게 만듬
    10) Coroutine의 while문 안에 조건문을 추가하여 플레이어가 스킵버튼을 누르면 대본을 바로 출력할 수 있게 만듬 (스킵 버튼: Space Bar)
    11) 대본이 전부 출력이 되면 Coroutine에서 다음 스킵 버튼 입력을 받을 때까지 대기함
    12) 스킵 버튼 입력이 들어오면 다음 대본이 있는지 확인을 함
    13-a) 다음 대본이 있다면 대본을 출력하게 만듬(다시 9에서 시작)
    13-b) 다음 대본이 없다면 대화를 종료함
    14) 대화가 끝나고 OnTriggerExit이면 말풍선을 숨기고 받은 대본 데이터와 NPC의 정보를 초기화시킴

    + 말풍선UI와 상호작용 활성화 이미지는 재활용함 (말풍선을 1개만 활성화 하는 것으로 상정)
    */
    [SerializeField]
    private DialogueProcess process = null;

    private DialoguePlayer player = null;
    private DialogueProfile[] profiles = null;
    private DialogueProfile profile = null;


    private void Awake()
    {
        // DialogueManager가 DialogueProfile을 하나도 모르므로 모든 DialogueProfile 호출하여 알게 만듬
        profiles = FindObjectsOfType<DialogueProfile>();
#if UNITY_EDITOR
        //Debug.LogFormat("[DialogueManager] Profile Count: {0}", profiles.Length);
#endif
    }
    private void Start()
    {
        //player = FindObjectOfType<DialoguePlayer>();
    }


    public void Init(
        DialogueProfile.OnTriggerEnterCallback _enterCallback,
        DialogueProfile.OnTriggerExitCallback _exitCallback,
        DialogueProcess.OnStartDialogue _startCallback,
        DialoguePlayer.SkipCallback _skipCallback
        )
    {
#if UNITY_EDITOR
        //Debug.Log("[DialogueManager] Init");
#endif
        player = FindObjectOfType<DialoguePlayer>();
        process.Init(_startCallback, IsDialOver);
        player.Init(
            _skipCallback,
            IsAction,
            StartDialogueProcess
            );

        for(int i = 0; i < profiles.Length; ++i)
        {
            // DialogueProfile Callback 초기화
            profiles[i].Init(_enterCallback, TargetProfile, _exitCallback, TargetLost);
        }

    }


    // 플레이어와 대화 상호작용을 하는 타겟의 프로필을 저장
    public void TargetProfile(DialogueProfile _profile)
    {
        profile = _profile;
    }
    // 플레이어와 대화 상호작용 상태에서 벗어나면 타겟의 프로필 제거
    public void TargetLost()
    {
        profile = null;
    }

    // 플레이어로부터 상호작용 여부를 확인하여 DialogueProcess를 진행
    public void StartDialogueProcess()
    {
        if (profile == null) return;

        if (profile.IsNPC() == false) return; // NPC가 아니면 대화를 하지 않습니다.

        string sid = profile.GetSid();
        int tid = profile.GetTid();
        process.StartDialogueProcess(sid, tid);
    }

    #region DialogueProfile Func
    // 씬 대본 아이디
    public string GetSid()
    {
        if (profile == null) return string.Empty;

        return profile.GetSid();
    }
    // NPC 대사 ID
    public int GetTid()
    {
        if (profile == null) return -1;

        return profile.GetTid();
    }
    // NPC인지 확인
    public bool IsNPC()
    {
        if (profile == null) return false;

        return profile.IsNPC();
    }
    // 상호작용의 대상이 되는 NPC의 상호작용 여부
    public bool IsAction()
    {
        if (profile == null) return false;

        return profile.IsAction();
    }
    public void IsAction(bool _action)
    {
        if (profile == null) return;

        profile.IsAction(_action);
    }
    public bool IsDialOver()
    {
        if (profile == null) return false;

        return profile.IsDialOver();
    }
    public void IsDialOver(bool _over)
    {
        if (profile == null) return;

        profile.IsDialOver(_over);
    }
    #endregion
}
