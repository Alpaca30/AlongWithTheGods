using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooltimeManager : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas = null;
    [SerializeField]
    private UICooltimeHolder[] cooltimeHolders = null;
    [SerializeField]
    private UICooltimeHolder prefab = null;

    private UICooltimeHolder dashHolder = null;
    private UICooltimeHolder skillHolder = null;


    private void Awake()
    {
        if(cooltimeHolders.Length <= 0)
        {
            Vector2 startPos = new Vector2(-120f, 80f);
            float offsetX = 30f;

            for(int i = 0; i < 2; ++i)
            {
                UICooltimeHolder holder = Instantiate(prefab, canvas.transform);

                RectTransform rt = null;
                if(holder.TryGetComponent<RectTransform>(out rt))
                {
                    rt.sizeDelta = new Vector2(100f, 100f);
                    rt.anchoredPosition = new Vector2();
                    rt.anchorMax = new Vector2(1f, 0f);
                    rt.anchorMin = new Vector2(1f, 0f);
                    rt.anchoredPosition = startPos;

                    startPos.x -= offsetX + rt.sizeDelta.x;
                }

                holder.SetSkillType((UICooltimeHolder.ESkillType)i);
                SetCooltimeHolder(holder);

                cooltimeHolders[i] = holder;
            }
        }
        else
        {
            for (int i = 0; i < cooltimeHolders.Length; ++i)
            {
                cooltimeHolders[i].gameObject.SetActive(true);
                SetCooltimeHolder(cooltimeHolders[i]);
            }
        }
    }


    public void Init(
        UICooltimeHolder.OnSkillCallback _dashCallback,
        UICooltimeHolder.OnSkillCallback _skillCallback
        )
    {
        if (cooltimeHolders == null || cooltimeHolders.Length <= 0)
        {
#if UNITY_EDITOR
            //Debug.LogError("[UICooltimeManager] UICooltimeHolder is Not Set.");
#endif
            return;
        }

        // 각 Holder들에게 스킬을 부여함
        for (int i = 0; i < cooltimeHolders.Length; ++i)
        {
            switch(cooltimeHolders[i].GetSkillType())
            {
                case UICooltimeHolder.ESkillType.Dash:
                    cooltimeHolders[i].Init(_dashCallback);
                    break;
                case UICooltimeHolder.ESkillType.Skill_1:
                    cooltimeHolders[i].Init(_skillCallback);
                    break;
            }
        }
    }


    /*
        UICooltimeHolder.OnSkillCallback _skillCallback | 유형에 따른 플레이어 스킬
        UICooltimeHolder.ESkillType _type | 스킬 유형 | Skill_1 = 1, Skill_2 = 2
    */
    public void OnChangeSkillEvent(UICooltimeHolder.OnSkillCallback _skillCallback, UICooltimeHolder.ESkillType _type)
    {
        skillHolder.SetSkillEvent(_skillCallback, _type);
    }
    public void OnChangeSkillEvent(UICooltimeHolder.OnSkillCallback _skillCallback)
    {
        skillHolder.SetSkillEvent(_skillCallback);
    }
    public void OnChangeSkillImage(UICooltimeHolder.ESkillType _type)
    {
        skillHolder.SetSkillType(_type);
    }

    /// <summary>
    /// UICooltimeHolder.ESkillType _type | CooltimeHolder Type | Dash = 0, Skill_1 = 1, Skill_2 = 2<br/>
    /// float _amount | 쿨타임 FillAmount값 | 0.0 ~ 1.0
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_amount"></param>
    public void Cooldown(UICooltimeHolder.ESkillType _type, float _amount)
    {
        if (_type == UICooltimeHolder.ESkillType.Dash)
            dashHolder.Cooldown(_amount);
        else if (_type == UICooltimeHolder.ESkillType.Skill_1 ||
                _type == UICooltimeHolder.ESkillType.Skill_2)
            skillHolder.Cooldown(_amount);
    }
    public void DashCooldown(float _amount)
    {
        dashHolder.Cooldown(_amount);
    }
    public void SkillCooldown(float _amount)
    {
        skillHolder.Cooldown(_amount);
    }

    /// <summary>
    /// Dash UI 활성화<br/>
    /// UICooltimeHolder.ESkillType _type | CooltimeHolder Type | Dash = 0, Skill_1 = 1, Skill_2 = 2
    /// </summary>
    /// <param name="_type"></param>
    public void ActiveSkill(UICooltimeHolder.ESkillType _type)
    {
        if (_type == UICooltimeHolder.ESkillType.Dash)
            dashHolder.Active();
        else if (_type == UICooltimeHolder.ESkillType.Skill_1 ||
                _type == UICooltimeHolder.ESkillType.Skill_2)
            skillHolder.Active();
    }
    public void ActiveDash()
    {
        dashHolder.Active();
    }
    public void ActiveSkill()
    {
        skillHolder.Active();
    }

    /*
        Skill UI 활성화/비활성화
        UICooltimeHolder.ESkillType _type | CooltimeHolder Type | Dash = 0, Skill_1 = 1, Skill_2 = 2
        bool _active | 활성화/비활성화
    */
    public void SkillActivity(UICooltimeHolder.ESkillType _type, bool _active)
    {
        if (_type == UICooltimeHolder.ESkillType.Dash)
            dashHolder.SkillActive(_active);
        else if (_type == UICooltimeHolder.ESkillType.Skill_1 ||
                _type == UICooltimeHolder.ESkillType.Skill_2)
            skillHolder.SkillActive(_active);
    }
    public void DashActivity(bool _active)
    {
        dashHolder.SkillActive(_active);
    }
    public void SkillActivity(bool _active)
    {
        skillHolder.SkillActive(_active);
    }

    // Dash UI 비활성화
    public void InactiveSkill(UICooltimeHolder.ESkillType _type)
    {
        if (_type == UICooltimeHolder.ESkillType.Dash)
            dashHolder.Inactive();
        else if (_type == UICooltimeHolder.ESkillType.Skill_1 ||
                _type == UICooltimeHolder.ESkillType.Skill_2)
            skillHolder.Inactive();
    }
    public void InactiveDash()
    {
        dashHolder.Inactive();
    }
    public void InactiveSkill()
    {
        skillHolder.Inactive();
    }

    public UICooltimeHolder GetCooltimeHolderByType(UICooltimeHolder.ESkillType _type)
    {
        switch (_type)
        {
            case UICooltimeHolder.ESkillType.Dash:
                return dashHolder;
            case UICooltimeHolder.ESkillType.Skill_1:
                return skillHolder;
            case UICooltimeHolder.ESkillType.Skill_2:
                return skillHolder;
        }
        return null;
    }

    private void SetCooltimeHolder(UICooltimeHolder _holder)
    {
        UICooltimeHolder.ESkillType type = _holder.GetSkillType();
        switch (type)
        {
            case UICooltimeHolder.ESkillType.Dash:
                dashHolder = _holder;
                break;
            case UICooltimeHolder.ESkillType.Skill_1:
                skillHolder = _holder;
                break;
            case UICooltimeHolder.ESkillType.Skill_2:
                skillHolder = _holder;
                break;
        }
    }
}