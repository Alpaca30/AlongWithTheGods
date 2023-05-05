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
        UICooltimeHolder.OnSkillCallback _dashCallback              // Player - Player�� Dash ��� �Լ�
        UICooltimeHolder.OnSkillCallback _skillCallback             // Player - Player�� Skill ��� �Լ�
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
            uiMg.SetPlayerHp,       // Player�� HP�� ����
            uiMg.SetPlayerMaxHp,
            uiMg.SetBossHp,         // ���� HP(����)�� ����
            uiMg.SetComboText,      // Combo�� ����
            uiMg.SkillActivity,
            uiMg.DashActivity,
            uiMg.SetActiveBossHp    // ������ ü�¹� Ȱ��ȭ/��Ȱ��ȭ
            );

        if (sceneEventMg != null)
            sceneEventMg.Init();

        dialMg.Init(
            uiMg.MoveUIToTarget,
            uiMg.DialogueUILostTarget,
            uiMg.StartDialogue,
            uiMg.SkipScriptUI          // ��ȭ ��ŵ
            );

        uiMg.Init(
            dialMg.IsAction,                    // ��ȭ������ Ȯ��
            dialMg.IsDialOver,                  // �ش� NPC�� ��ȭ�� �����ƴ��� ����
            sceneEventMg.OpenPortal,             // ��ȭ �� ��Ż ����
            gameCtrlMg.UseDash,                 // Dash ��ų
            gameCtrlMg.UseSkill                // ��ų �б��� �Լ� ����
            );
    }
}
