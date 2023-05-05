using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [SerializeField]
    private Effect[] effects;

    private List<GameObject> listEffects = new List<GameObject>();


    private void Awake()
    {
        Instance = this;
    }


    /// <summary>
    /// Ÿ���� ������� ��ϵ� Effect�� ���<br/>
    /// <paramref name="_pos"/>�� Effect�� ��ġ���� ����<br/>
    /// <paramref name="_rot"/>�� Effect�� ȸ������ ����<br/> 
    /// <br/>
    /// Effect.EActorType <paramref name="_actorType"/> | ����� Ÿ�� | Player, Monster, Boss, Environment, Custom <br/>
    /// Effect.EActionType <paramref name="_actionType"/> | ���� Ÿ�� | Appear, Idle, Attack, Slash, Hit, Dash, Jump, Run, Etc <br/>
    /// Effect.EAttackType <paramref name="_attackType"/> | ���� Ÿ�� | None, Melee, LongDistance, Magic <br/>
    /// Vector3 <paramref name="_pos"/> | Effect�� Position �� <br/>
    /// Quaternion <paramref name="_rot"/> | Effect�� Rotation �� (Quaternion ���) <br/>
    /// int <paramref name="_idx"/> | ��ϵ� Effect�� ���� ���� ����� index�� | default: 0 <br/>
    /// float <paramref name="_time"/> | ���� ��� �ð� | default: 0<br/>
    /// </summary>
    /// <param name="_actorType">����� Ÿ�� | Player, Monster, Boss, Environment</param>
    /// <param name="_actionType">���� Ÿ�� | Appear, Idle, Attack, Slash, Hit, Dash, Jump, Run, Etc</param>
    /// <param name="_attackType">���� Ÿ�� | None, Melee, LongDistance, Magic</param>
    /// <param name="_pos">Effect�� Position �� | default: new Vector3()</param>
    /// <param name="_rot">Effect�� Quaternion ��(Rotation) | default: new Quaternion()</param>
    /// <param name="_idx">��ϵ� Effect�� ���� ���� ����� index�� | default: 0</param>
    /// <param name="_time">���� ��� �ð� | default: 0</param>
    public void Play(
        Effect.EActorType _actorType,
        Effect.EActionType _actionType,
        Effect.EAttackType _attackType,
        Vector3 _pos,
        Quaternion _rot,
        int _idx = 0,
        float _time = 0f
        )
    {
        Effect effect = Array.Find(effects, dummyEffect => (
            dummyEffect.actorType == _actorType &&
            dummyEffect.actionType == _actionType &&
            dummyEffect.attackType == _attackType
        ));
        WaitForPlay(effect, _pos, _rot, _idx, _time);
    }
    /// <summary>
    /// Ÿ���� ������� ��ϵ� Effect�� ���<br/>
    /// <paramref name="_parent"/>�� Position�� Rotation�� ������� ������<br/>
    /// <br/>
    /// Effect.EActorType <paramref name="_actorType"/> | ����� Ÿ�� | Player, Monster, Boss, Environment, Custom <br/>
    /// Effect.EActionType <paramref name="_actionType"/> | ���� Ÿ�� | Appear, Idle, Attack, Slash, Hit, Dash, Jump, Run, Etc <br/>
    /// Effect.EAttackType <paramref name="_attackType"/> | ���� Ÿ�� | None, Melee, LongDistance, Magic <br/>
    /// Transform <paramref name="_parent"/> | Effect�� ������ �θ��� ��ġ <br/>
    /// int <paramref name="_idx"/> | ��ϵ� Effect�� ���� ���� ����� index�� | default: 0 <br/>
    /// float <paramref name="_time"/> | ���� ��� �ð� | default: 0<br/>
    /// </summary>
    /// <param name="_actorType">����� Ÿ�� | Player, Monster, Boss, Environment</param>
    /// <param name="_actionType">���� Ÿ�� | Appear, Idle, Attack, Slash, Hit, Dash, Jump, Run, Etc</param>
    /// <param name="_attackType">���� Ÿ�� | None, Melee, LongDistance, Magic</param>
    /// <param name="_parent">Effect�� ������ �θ��� ��ġ</param>
    /// <param name="_idx">��ϵ� Effect�� ���� ���� ����� index�� | default: 0</param>
    /// <param name="_time">���� ��� �ð� | default: 0</param>
    public void Play(
        Effect.EActorType _actorType,
        Effect.EActionType _actionType,
        Effect.EAttackType _attackType,
        Transform _parent,
        int _idx = 0,
        float _time = 0f
        )
    {
        Effect effect = Array.Find(effects, dummyEffect => (
            dummyEffect.actorType == _actorType &&
            dummyEffect.actionType == _actionType &&
            dummyEffect.attackType == _attackType
        ));
        WaitForPlay(effect, _parent, _idx, _time);
    }


    /// <summary>
    /// ����� �̸��� ������� ��ϵ� Effect�� ���<br/>
    /// <paramref name="_parent"/>�� Position�� Rotation�� ������� ������<br/>
    /// <br/>
    /// string <paramref name="_effectName"/> | ����� Effect�� �̸� <br/>
    /// Transform <paramref name="_parent"/> | Effect�� ������ �θ��� ��ġ <br/>
    /// int <paramref name="_idx"/> | ��ϵ� Effect�� ���� ���� ����� index�� | default: 0 <br/>
    /// float <paramref name="_time"/> | ���� ��� �ð� | default: 0<br/>
    /// </summary>
    /// <param name="_effectName">����� Effect�� �̸�</param>
    /// <param name="_parent">Effect�� ������ �θ��� ��ġ</param>
    /// <param name="_idx">��ϵ� Effect�� ���� ���� ����� index�� | default: 0</param>
    /// <param name="_time">���� ��� �ð� | default: 0</param>
    public void Play(string _effectName, Transform _parent, int _idx = 0, float _time = 0f)
    {
        Effect effect = FindEffect(_effectName);
        WaitForPlay(effect, _parent, _idx, _time);
    }
    /// <summary>
    /// ����� �̸��� ������� ��ϵ� Effect�� ���<br/>
    /// <paramref name="_pos"/>�� Effect�� ��ġ���� ����<br/>
    /// <paramref name="_rot"/>�� Effect�� ȸ������ ����<br/> 
    /// <br/>
    /// string <paramref name="_effectName"/> | ����� Effect�� �̸� <br/>
    /// Vector3 <paramref name="_pos"/> | Effect�� Position �� <br/>
    /// Quaternion <paramref name="_rot"/> | Effect�� Rotation �� (Quaternion ���) <br/>
    /// int <paramref name="_idx"/> | ��ϵ� Effect�� ���� ���� ����� index�� | default: 0 <br/>
    /// float <paramref name="_time"/> | ���� ��� �ð� | default: 0<br/>
    /// </summary>
    /// <param name="_effectName">����� Effect�� �̸�</param>
    /// <param name="_pos">Effect�� Position �� | default: new Vector3()</param>
    /// <param name="_rot">Effect�� Quaternion ��(Rotation) | default: new Quaternion()</param>
    /// <param name="_idx">��ϵ� Effect�� ���� ���� ����� index�� | default: 0</param>
    /// <param name="_time">���� ��� �ð� | default: 0</param>
    public void Play(string _effectName, Vector3 _pos, Quaternion _rot, int _idx = 0, float _time = 0f)
    {
        Effect effect = FindEffect(_effectName);
        WaitForPlay(effect, _pos, _rot, _idx, _time);
    }


    /// <summary>
    /// ����� Effect�� �̸��� ���� Effect�� ����<br/>
    /// <br/>
    /// string <paramref name="_effectName"/> | ����� Effect�� �̸� <br/>
    /// float <paramref name="_time"/> | ���� ���� �ð� | default: 0<br/>
    /// </summary>
    /// <param name="_effectName">����� Effect�� �̸�</param>
    /// <param name="_time">���� ���� �ð� | default: 0</param>
    public void Stop(string _effectName, float _time = 0f)
    {
        //GameObject go = listEffects.Find(dummyEffect => dummyEffect.name == _effectName);
        for(int i = 0; i < listEffects.Count; ++i)
        {
            if (listEffects[i] == null) continue;

#if UNITY_EDITOR
            Debug.LogFormat("GameObject go[{0}]: {1}({2})", i, listEffects[i].name, listEffects[i]);
#endif
            if (listEffects[i].name == _effectName)
            {
                GameObject go = listEffects[i];

                if(go != null)
                    Destroy(go, _time);
            }
        }
    }


    private Effect FindEffect(string _name)
    {
        return Array.Find(effects, dummyEffect => dummyEffect.effectName == _name);
    }

    private void PlayProcess(Effect _effect, Transform _parent, int _idx)
    {
        if (_effect != null)
        {
            if (_effect.effects.Length <= 0) return;

            if (_idx < 0) _idx = 0;
            else if (_idx >= _effect.effects.Length) _idx = _effect.effects.Length - 1;

            GameObject go = Instantiate(_effect.effects[_idx], _parent);
            go.name = _effect.effectName;
            listEffects.Add(go);

            ParticleSystem ps = go.GetComponentInChildren<ParticleSystem>();
            if (ps.isPlaying == false)
                ps.Play();
        }
    }
    private void PlayProcess(Effect _effect, Vector3 _pos, Quaternion _rot, int _idx)
    {
        if (_effect != null)
        {
            if (_effect.effects.Length <= 0) return;

            if (_idx < 0) _idx = 0;
            else if (_idx >= _effect.effects.Length) _idx = _effect.effects.Length - 1;

            GameObject go = Instantiate(_effect.effects[_idx], _pos, _rot);
            go.name = _effect.effectName;
            listEffects.Add(go);

            ParticleSystem ps = go.GetComponentInChildren<ParticleSystem>();
            if (ps.isPlaying == false)
                ps.Play();
        }
    }


    private void WaitForPlay(
        Effect _effect,
        Transform _parent,
        int _idx, float _time
        )
    {
        StartCoroutine(
            WaitForPlayCoroutine(_effect, _parent, _idx, _time)
            );
    }
    private void WaitForPlay(
        Effect _effect,
        Vector3 _pos, Quaternion _rot,
        int _idx, float _time
        )
    {
        StartCoroutine(
            WaitForPlayCoroutine(_effect, _pos, _rot, _idx, _time)
            );
    }

    private IEnumerator WaitForPlayCoroutine(
        Effect _effect,
        Transform _parent,
        int _idx, float _time
        )
    {
        yield return new WaitForSeconds(_time);
        PlayProcess(_effect, _parent, _idx);
    }
    private IEnumerator WaitForPlayCoroutine(
        Effect _effect,
        Vector3 _pos, Quaternion _rot,
        int _idx, float _time
        )
    {
        yield return new WaitForSeconds(_time);
        PlayProcess(_effect, _pos, _rot, _idx);
    }
}
