using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameControlManager gameCtrlMg = null;
    [SerializeField]
    private UIManager uiMg = null;
    [SerializeField]
    private DialogueManager dialMg = null;
    [SerializeField]
    private SceneEventsManager sceneEventMg = null;


    private void Start()
    {
        Init();
    }


    /*
        * DialogueManager
        DialogueProfile.OnTriggerEnterCallback _enterCallback   // DialogueProfile - UIDialogueManager(MoveUIToTarget)
        DialogueProfile.OnTriggerExitCallback _exitCallback     // DialogueProfile - UIDialogueManager(LostUITarget)
        DialogueProcess.OnStartDialogue _startCallback          // DialogueProcess - UIDialogueManager(StartCommunication)
        DialoguePlayer.SkipCallback _skipCallback               // DialoguePlayer - UIDialogueManager(SkipScript)

        * UIManager
        UIDialogueManager.OnDialogueStartCallback _startCallback    // UIDialogueManager - DialogueProfile(IsAction)
        UIHpController.OnChangeHpCallback _hpCallback               // UIHpController - Player(GetHp)
        UICooltimeHolder.OnSkillCallback _dashCallback              // Player - Player의 Dash 사용 함수
        UICooltimeHolder.OnSkillCallback _skillCallback             // Player - Player의 Skill 사용 함수
    */
    private void Init()
    {
        // Player Hp
        // Player MaxHp
        // Boss HP
        // Combo Count
        // Skill Active
        // Dash Active

        gameCtrlMg.Init(
            uiMg.SetPlayerHp,       // Player의 HP를 받음
            uiMg.SetPlayerMaxHp,
            uiMg.SetBossHp,         // 보스 HP(비율)을 받음
            uiMg.SetComboText,      // Combo를 받음
            uiMg.SkillActivity,
            uiMg.DashActivity,
            uiMg.SetActiveBossHp    // 보스의 체력바 활성화/비활성화
            );

        if (sceneEventMg != null)
            sceneEventMg.Init();

        dialMg.Init(
            uiMg.MoveUIToTarget,
            uiMg.DialogueUILostTarget,
            uiMg.StartDialogue,
            uiMg.SkipScriptUI          // 대화 스킵
            );

        uiMg.Init(
            dialMg.IsAction,                    // 대화중인지 확인
            dialMg.IsDialOver,                  // 해당 NPC와 대화를 끝마쳤는지 설정
            sceneEventMg.OpenPortal,             // 대화 후 포탈 열기
            gameCtrlMg.UseDash,                 // Dash 스킬
            gameCtrlMg.UseSkill                // 스킬 분기사용 함수 받음
            );
    }
}
