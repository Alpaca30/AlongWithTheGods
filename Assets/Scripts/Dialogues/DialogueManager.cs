using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    /*
    1) �÷��̾ NPC �� OnTriggerEnter�� �ߵ��ϸ� ��ȣ�ۿ� ������ Ȱ��ȭ ��Ŵ
    1-a) �÷��̾�� NPC ���� ��ȭ ��ȣ�ۿ� �ߵ� (�ӽ� ��ȣ�ۿ� ��ư: Space Bar)
    2) ��ȣ�ۿ��ϴ� NPC�� ������ �����ͼ� ����
    3) NPC�� ������ ���� ��ȭ������ FileManager���� �о������ ��
    4) �ҷ��� �뺻�� DialogueProcess���� Json���·� Deserialize�� �ؼ� ����ü�� ������
    5) Json���·� ������ �뺻 ������ DialogueManager�� ������
    6) DialogueManager���� UIDialogueManager�� �뺻 ������ NPC�� ������ �ѱ�
    7) UIDialogueManager������ ���ܵ� ��ǳ�� ��ġ�� �Ѱܹ��� Position���� �̵���Ŵ
    8) �̵� �� ���ܵ� ��ǳ���� ���̰� ����
    9) �뺻�� Coroutine�� �̿��Ͽ� ����ϰ� ����
    10) Coroutine�� while�� �ȿ� ���ǹ��� �߰��Ͽ� �÷��̾ ��ŵ��ư�� ������ �뺻�� �ٷ� ����� �� �ְ� ���� (��ŵ ��ư: Space Bar)
    11) �뺻�� ���� ����� �Ǹ� Coroutine���� ���� ��ŵ ��ư �Է��� ���� ������ �����
    12) ��ŵ ��ư �Է��� ������ ���� �뺻�� �ִ��� Ȯ���� ��
    13-a) ���� �뺻�� �ִٸ� �뺻�� ����ϰ� ����(�ٽ� 9���� ����)
    13-b) ���� �뺻�� ���ٸ� ��ȭ�� ������
    14) ��ȭ�� ������ OnTriggerExit�̸� ��ǳ���� ����� ���� �뺻 �����Ϳ� NPC�� ������ �ʱ�ȭ��Ŵ

    + ��ǳ��UI�� ��ȣ�ۿ� Ȱ��ȭ �̹����� ��Ȱ���� (��ǳ���� 1���� Ȱ��ȭ �ϴ� ������ ����)
    */
    [SerializeField]
    private DialogueProcess process = null;

    private DialoguePlayer player = null;
    private DialogueProfile[] profiles = null;
    private DialogueProfile profile = null;


    private void Awake()
    {
        // DialogueManager�� DialogueProfile�� �ϳ��� �𸣹Ƿ� ��� DialogueProfile ȣ���Ͽ� �˰� ����
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
            // DialogueProfile Callback �ʱ�ȭ
            profiles[i].Init(_enterCallback, TargetProfile, _exitCallback, TargetLost);
        }

    }


    // �÷��̾�� ��ȭ ��ȣ�ۿ��� �ϴ� Ÿ���� �������� ����
    public void TargetProfile(DialogueProfile _profile)
    {
        profile = _profile;
    }
    // �÷��̾�� ��ȭ ��ȣ�ۿ� ���¿��� ����� Ÿ���� ������ ����
    public void TargetLost()
    {
        profile = null;
    }

    // �÷��̾�κ��� ��ȣ�ۿ� ���θ� Ȯ���Ͽ� DialogueProcess�� ����
    public void StartDialogueProcess()
    {
        if (profile == null) return;

        if (profile.IsNPC() == false) return; // NPC�� �ƴϸ� ��ȭ�� ���� �ʽ��ϴ�.

        string sid = profile.GetSid();
        int tid = profile.GetTid();
        process.StartDialogueProcess(sid, tid);
    }

    #region DialogueProfile Func
    // �� �뺻 ���̵�
    public string GetSid()
    {
        if (profile == null) return string.Empty;

        return profile.GetSid();
    }
    // NPC ��� ID
    public int GetTid()
    {
        if (profile == null) return -1;

        return profile.GetTid();
    }
    // NPC���� Ȯ��
    public bool IsNPC()
    {
        if (profile == null) return false;

        return profile.IsNPC();
    }
    // ��ȣ�ۿ��� ����� �Ǵ� NPC�� ��ȣ�ۿ� ����
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
