using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private UIDialogueManager uiDialMg = null;
    [SerializeField]
    private UIStatusManager uiStatMg = null;
    [SerializeField]
    private UICooltimeManager uiCoolMg = null;
    [SerializeField]
    private UIComboManager uiComboMg = null;

    //[SerializeField]
    //private Canvas[] canvases = null;


    private void Start()
    {
        // 문제 있어서 작동 중단
        //SyncCanvasesResolution();
    }


    public void Init(
        UIDialogueManager.OnDialogueStartCallback _startDialogueCallback, // DialogueProfile - NPC가 대화상태에 들어갔는지 확인
        UIDialogueManager.OnDialogueOverCallback _dialogueOverCallback, // DialogueProfile - NPC와 대화를 끝마쳤는지 확인
        UIDialogueManager.OnDialogueEndCallback _dialogueEndCallback, // SceneEventsManager - NPC와 대화를 끝마쳤다면 포탈을 엶.
        UICooltimeHolder.OnSkillCallback _dashCallback, // Player - Player의 Dash 사용 함수
        UICooltimeHolder.OnSkillCallback _skillCallback // Player - Player의 Skill 사용 함수
        )
    {
#if UNITY_EDITOR
        //Debug.Log("[UIManager] Init");
#endif
        uiDialMg.Init(_startDialogueCallback, _dialogueOverCallback, _dialogueEndCallback);
        uiStatMg.Init();
        uiCoolMg.Init(_dashCallback, _skillCallback);
        uiComboMg.Init(uiCoolMg.OnChangeSkillImage);
    }


    ///*
    //    Canvas의 Reference 해상도를 현재 해상도와 동일하게 변경
    //*/
    //public void SyncCanvasesResolution()
    //{
    //    if (canvases == null || canvases.Length <= 0) return;

    //    Resolution resolution = Screen.currentResolution;
    //    for (int i = 0; i < canvases.Length; ++i)
    //    {
    //        CanvasScaler scaler = canvases[i].GetComponent<CanvasScaler>();
    //        if(scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
    //            scaler.referenceResolution = new Vector2(resolution.width, resolution.height);
    //    }
    //}
    ///*
    //    Canvas의 Reference 해상도를 현재 해상도와 동일하게 변경
    //    CanvasScaler _scaler | Canvas가 가지고 있는 CanvasScaler 컴포넌트
    //*/
    //public void SyncCanvasResolution(CanvasScaler _scaler)
    //{
    //    if (_scaler == null) return;

    //    Resolution resolution = Screen.currentResolution;
    //    _scaler.referenceResolution = new Vector2(resolution.width, resolution.height);
    //}


    #region UIDialogueManager Func
    // OnTriggerEnter이면 발동하여 말풍선과 상호작용 안내 문구를 대상으로 이동
    public void MoveUIToTarget(bool _isNpc, Transform _target)
    {
        uiDialMg.MoveUIToTarget(_isNpc, _target);
    }
    // OnTriggerExit이면 발동하여 말풍선과 상호작용 안내 문구 비활성화
    public void DialogueUILostTarget()
    {
        uiDialMg.LostUITarget();
    }
    // 상호작용(대화) 시작 (Player -> NPC)
    public void StartDialogue(int _tid, List<DialogueProcess.SDialogue> _scripts)
    {
        uiDialMg.StartDialogue(_tid, _scripts);
    }
    // 대화 스킵
    public void SkipScriptUI()
    {
        uiDialMg.SkipScript();
    }
    #endregion


    #region UICooltimeManager Func
    public void DashActivity(bool _active)
    {
        uiCoolMg.DashActivity(_active);
    }
    public void SkillActivity(bool _active)
    {
        uiCoolMg.SkillActivity(_active);
    }
    /*
        쿨타임 진행도를 조작
        UICooltimeHolder.ESkillType _type | Dash = 0, Skill_1 = 1, Skill_2 = 2
        float _amount | 쿨타임 진행도 | 0.0 ~ 1.0
    */
    public void Cooldown(UICooltimeHolder.ESkillType _type, float _amount)
    {
        uiCoolMg.Cooldown(_type, _amount);
    }
    /*
        유형에 따른 스킬 활성화
        UICooltimeHolder.ESkillType _type | Dash = 0, Skill_1 = 1, Skill_2 = 2
    */
    public void ActiveSkill(UICooltimeHolder.ESkillType _type)
    {
        uiCoolMg.ActiveSkill(_type);
    }
    /*
        유형에 따른 스킬 비활성화
        UICooltimeHolder.ESkillType _type | Dash = 0, Skill_1 = 1, Skill_2 = 2
    */
    public void InactiveSkill(UICooltimeHolder.ESkillType _type)
    {
        uiCoolMg.InactiveSkill(_type);
    }
    /*
        콤보에 따라 스킬이 변형되는 형태이기에 SkillCallback이 일치하는지 확인하기 편하게 추가적으로 SkillType 유형을 매개변수로 받음.
        UICooltimeHolder.OnSkillCallback _skillCallback | 사용될 Skill
        UICooltimeHolder.ESkillType _type | Skill 유형 | Skill_1 = 1, Skill_2 = 2
    */
    public void OnChangeSkillUI(UICooltimeHolder.OnSkillCallback _skillCallback, UICooltimeHolder.ESkillType _type)
    {
        uiCoolMg.OnChangeSkillEvent(_skillCallback, _type);
    }
    #endregion


    #region UIComboManager Func
    public void SetComboText(int _combo)
    {
        uiComboMg.SetComboText(_combo);
    }
    #endregion


    #region UIStatusManager Func
    public void SetPlayerHp(int _curHp)
    {
        uiStatMg.SetPlayerHp(_curHp);
    }
    public void SetPlayerMaxHp(int _maxHp)
    {
        uiStatMg.SetPlayerMaxHp(_maxHp);
    }

    public void SetActiveBossHp(bool _active)
    {
        uiStatMg.SetActiveBossHp(_active);
    }
    public void SetBossHp(float _curHpRate)
    {
        uiStatMg.SetBossHp(_curHpRate);
    }
    #endregion
}
