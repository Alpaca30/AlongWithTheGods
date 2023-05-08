using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooltimeHolder : MonoBehaviour
{
    // Manager는 다수의 Holder를 보유하므로 누가 실행되는지를 Holder에서 자기자신을 담아서 Callback처리함
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

        loader.raycastTarget = false; // 선택이 되지 않도록 만듬
        loader.fillAmount = 0f; // 0으로 초기화
    }


    /*
        OnSkillCallback _skillCallback | Skill이 Cooltime에 들어가는 함수
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
        skill.sprite = GetSprite(); // Type에 맞는 Sprite를 가져옴
    }
    #endregion


    public void SetSkillEvent(OnSkillCallback _skillCallback)
    {
        if (_skillCallback == null) return;

        skillCallback = _skillCallback;

        Button btn = null;
        if (skill.TryGetComponent<Button>(out btn))
        {
            btn.onClick.RemoveAllListeners(); // Listener 초기화
            btn.onClick.AddListener(() => skillCallback?.Invoke()); // 각 스킬을 받아서 처리함
        }
    }
    public void SetSkillEvent(OnSkillCallback _skillCallback, ESkillType _type)
    {
        SetSkillEvent(_skillCallback);
        SetSkillType(_type);
    }


    // 쿨타임 Toggle //
    // 스킬 버튼 활성화
    public void Active()
    {
        SkillActivity(true);
    }
    // 스킬 버튼 비활성화
    public void Inactive()
    {
        SkillActivity(false);
    }
    public void SkillActive(bool _active)
    {
        SkillActivity(_active);
    }
    // float _amount | 쿨타임 진행도를 제공받음 | 0.0f ~ 1.0f
    public void Cooldown(float _amount)
    {
        if (uiCooltime == null) return;

        uiCooltime.SetCooltimeProgress(_amount);
    }


    // 현재 SkillType에 따른 Sprite 반환
    // return | Sprite
    private Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/UI/Skills/" + skillType.ToString());
    }

    // bool _active | 스킬 활성/비활성
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

    // 스킬 활성화 시 Alpha 조정
    private void SkillActive()
    {
        SetAlpha(skill, 1f);
        SetAlpha(border, 1f);
    }

    // 스킬 비활성화 시 Alpha 조정
    private void SkillInActive()
    {
        SetAlpha(skill, 0.5f);
        SetAlpha(border, 0.5f);
    }

    // Image _img | Alpha값을 조절할 Image
    // float _alpha | Alpha값 | 0.0f ~ 1.0f
    private void SetAlpha(Image _img, float _alpha)
    {
        if (_alpha > 1) _alpha = 1;
        else if (_alpha < 0) _alpha = 0;
        
        Color c = _img.color;
        c.a = _alpha;
        _img.color = c;
    }
}
