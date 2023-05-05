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
        // ���� �־ �۵� �ߴ�
        //SyncCanvasesResolution();
    }


    public void Init(
        UIDialogueManager.OnDialogueStartCallback _startDialogueCallback, // DialogueProfile - NPC�� ��ȭ���¿� ������ Ȯ��
        UIDialogueManager.OnDialogueOverCallback _dialogueOverCallback, // DialogueProfile - NPC�� ��ȭ�� �����ƴ��� Ȯ��
        UIDialogueManager.OnDialogueEndCallback _dialogueEndCallback, // SceneEventsManager - NPC�� ��ȭ�� �����ƴٸ� ��Ż�� ��.
        UICooltimeHolder.OnSkillCallback _dashCallback, // Player - Player�� Dash ��� �Լ�
        UICooltimeHolder.OnSkillCallback _skillCallback // Player - Player�� Skill ��� �Լ�
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
    //    Canvas�� Reference �ػ󵵸� ���� �ػ󵵿� �����ϰ� ����
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
    //    Canvas�� Reference �ػ󵵸� ���� �ػ󵵿� �����ϰ� ����
    //    CanvasScaler _scaler | Canvas�� ������ �ִ� CanvasScaler ������Ʈ
    //*/
    //public void SyncCanvasResolution(CanvasScaler _scaler)
    //{
    //    if (_scaler == null) return;

    //    Resolution resolution = Screen.currentResolution;
    //    _scaler.referenceResolution = new Vector2(resolution.width, resolution.height);
    //}


    #region UIDialogueManager Func
    // OnTriggerEnter�̸� �ߵ��Ͽ� ��ǳ���� ��ȣ�ۿ� �ȳ� ������ ������� �̵�
    public void MoveUIToTarget(bool _isNpc, Transform _target)
    {
        uiDialMg.MoveUIToTarget(_isNpc, _target);
    }
    // OnTriggerExit�̸� �ߵ��Ͽ� ��ǳ���� ��ȣ�ۿ� �ȳ� ���� ��Ȱ��ȭ
    public void DialogueUILostTarget()
    {
        uiDialMg.LostUITarget();
    }
    // ��ȣ�ۿ�(��ȭ) ���� (Player -> NPC)
    public void StartDialogue(int _tid, List<DialogueProcess.SDialogue> _scripts)
    {
        uiDialMg.StartDialogue(_tid, _scripts);
    }
    // ��ȭ ��ŵ
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
        ��Ÿ�� ���൵�� ����
        UICooltimeHolder.ESkillType _type | Dash = 0, Skill_1 = 1, Skill_2 = 2
        float _amount | ��Ÿ�� ���൵ | 0.0 ~ 1.0
    */
    public void Cooldown(UICooltimeHolder.ESkillType _type, float _amount)
    {
        uiCoolMg.Cooldown(_type, _amount);
    }
    /*
        ������ ���� ��ų Ȱ��ȭ
        UICooltimeHolder.ESkillType _type | Dash = 0, Skill_1 = 1, Skill_2 = 2
    */
    public void ActiveSkill(UICooltimeHolder.ESkillType _type)
    {
        uiCoolMg.ActiveSkill(_type);
    }
    /*
        ������ ���� ��ų ��Ȱ��ȭ
        UICooltimeHolder.ESkillType _type | Dash = 0, Skill_1 = 1, Skill_2 = 2
    */
    public void InactiveSkill(UICooltimeHolder.ESkillType _type)
    {
        uiCoolMg.InactiveSkill(_type);
    }
    /*
        �޺��� ���� ��ų�� �����Ǵ� �����̱⿡ SkillCallback�� ��ġ�ϴ��� Ȯ���ϱ� ���ϰ� �߰������� SkillType ������ �Ű������� ����.
        UICooltimeHolder.OnSkillCallback _skillCallback | ���� Skill
        UICooltimeHolder.ESkillType _type | Skill ���� | Skill_1 = 1, Skill_2 = 2
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
