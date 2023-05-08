using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooltimeHolder : MonoBehaviour
{
    // Manager�� �ټ��� Holder�� �����ϹǷ� ���� ����Ǵ����� Holder���� �ڱ��ڽ��� ��Ƽ� Callbackó����
    public delegate void OnSkillCallback();
    private OnSkillCallback skillCallback = null;
    
    public enum ESkillType
    {
        Dash, Skill_1, Skill_2, Count
    };

    [SerializeField]
    private ESkillType skillType;

    private UICooltime uiCooltime = null;

    private Image skill = null; // Skill
    private Image border = null; // Border


    private void Awake()
    {
        Image[] imgs = GetComponentsInChildren<Image>();
        if (imgs.Length < 0)
        {
#if UNITY_EDITOR
            //Debug.LogWarning("[UICooltimeHolder] There are no children of Image.");
#endif
            return;
        }

        Sprite skillImg = GetSprite();
        Sprite bgImg = Resources.Load<Sprite>("Sprites/UI/Skills/Background");
        Sprite borderImg = Resources.Load<Sprite>("Sprites/UI/Skills/Border");
        Sprite loaderImg = Resources.Load<Sprite>("Sprites/UI/Skills/Loader");

        Image bg = imgs[0]; // Background
        skill = imgs[1]; // Skill
        border = imgs[2]; // Border
        Image loader = imgs[3]; // Loader

        if (loader.TryGetComponent<UICooltime>(out uiCooltime) == false)
        {
            Debug.LogWarning("Couldn't Get Skill UI Loader.");
        }

        bg.sprite = bgImg ? bgImg : null;
        skill.sprite = skillImg ? skillImg : null;
        border.sprite = borderImg ? borderImg : null;
        loader.sprite = loaderImg ? loaderImg : null;

        SetAlpha(bg, 0.2f);
        SkillActive();

        loader.raycastTarget = false; // ������ ���� �ʵ��� ����
        loader.fillAmount = 0f; // 0���� �ʱ�ȭ
    }


    /*
        OnSkillCallback _skillCallback | Skill�� Cooltime�� ���� �Լ�
    */
    public void Init(OnSkillCallback _skillCallback)
    {
        SetSkillEvent(_skillCallback, skillType);
        if (skillType == ESkillType.Dash)
            Active();
        else
            Inactive();
    }


    #region Getter/Setter
    public ESkillType GetSkillType()
    {
        return skillType;
    }
    public void SetSkillType(ESkillType _type)
    {
        if (_type < 0 || _type >= ESkillType.Count)
        {
            Debug.LogWarning("[UICooltimeHolder] No Match Types.");
            return;
        }

        skillType = _type;
        skill.sprite = GetSprite(); // Type�� �´� Sprite�� ������
    }
    #endregion


    public void SetSkillEvent(OnSkillCallback _skillCallback)
    {
        if (_skillCallback == null) return;

        skillCallback = _skillCallback;

        Button btn = null;
        if (skill.TryGetComponent<Button>(out btn))
        {
            btn.onClick.RemoveAllListeners(); // Listener �ʱ�ȭ
            btn.onClick.AddListener(() => skillCallback?.Invoke()); // �� ��ų�� �޾Ƽ� ó����
        }
    }
    public void SetSkillEvent(OnSkillCallback _skillCallback, ESkillType _type)
    {
        SetSkillEvent(_skillCallback);
        SetSkillType(_type);
    }


    // ��Ÿ�� Toggle //
    // ��ų ��ư Ȱ��ȭ
    public void Active()
    {
        SkillActivity(true);
    }
    // ��ų ��ư ��Ȱ��ȭ
    public void Inactive()
    {
        SkillActivity(false);
    }
    public void SkillActive(bool _active)
    {
        SkillActivity(_active);
    }
    // float _amount | ��Ÿ�� ���൵�� �������� | 0.0f ~ 1.0f
    public void Cooldown(float _amount)
    {
        if (uiCooltime == null) return;

        uiCooltime.SetCooltimeProgress(_amount);
    }


    // ���� SkillType�� ���� Sprite ��ȯ
    // return | Sprite
    private Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/UI/Skills/" + skillType.ToString());
    }

    // bool _active | ��ų Ȱ��/��Ȱ��
    private void SkillActivity(bool _active)
    {
        if (uiCooltime == null) return;

        Button btn = skill.GetComponent<Button>();
        btn.interactable = _active;
        
        uiCooltime.SetCooltimeProgress(0f);

        if (_active == true)
            SkillActive();
        else
            SkillInActive();
    }

    // ��ų Ȱ��ȭ �� Alpha ����
    private void SkillActive()
    {
        SetAlpha(skill, 1f);
        SetAlpha(border, 1f);
    }

    // ��ų ��Ȱ��ȭ �� Alpha ����
    private void SkillInActive()
    {
        SetAlpha(skill, 0.5f);
        SetAlpha(border, 0.5f);
    }

    // Image _img | Alpha���� ������ Image
    // float _alpha | Alpha�� | 0.0f ~ 1.0f
    private void SetAlpha(Image _img, float _alpha)
    {
        if (_alpha > 1) _alpha = 1;
        else if (_alpha < 0) _alpha = 0;
        
        Color c = _img.color;
        c.a = _alpha;
        _img.color = c;
    }
}
