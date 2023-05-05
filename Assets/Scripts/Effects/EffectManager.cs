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
    /// 타입을 기반으로 등록된 Effect를 재생<br/>
    /// <paramref name="_pos"/>는 Effect의 위치값을 설정<br/>
    /// <paramref name="_rot"/>은 Effect의 회전값을 설정<br/> 
    /// <br/>
    /// Effect.EActorType <paramref name="_actorType"/> | 사용자 타입 | Player, Monster, Boss, Environment, Custom <br/>
    /// Effect.EActionType <paramref name="_actionType"/> | 행위 타입 | Appear, Idle, Attack, Slash, Hit, Dash, Jump, Run, Etc <br/>
    /// Effect.EAttackType <paramref name="_attackType"/> | 공격 타입 | None, Melee, LongDistance, Magic <br/>
    /// Vector3 <paramref name="_pos"/> | Effect의 Position 값 <br/>
    /// Quaternion <paramref name="_rot"/> | Effect의 Rotation 값 (Quaternion 기반) <br/>
    /// int <paramref name="_idx"/> | 등록된 Effect에 대해 순차 실행될 index값 | default: 0 <br/>
    /// float <paramref name="_time"/> | 지연 재생 시간 | default: 0<br/>
    /// </summary>
    /// <param name="_actorType">사용자 타입 | Player, Monster, Boss, Environment</param>
    /// <param name="_actionType">행위 타입 | Appear, Idle, Attack, Slash, Hit, Dash, Jump, Run, Etc</param>
    /// <param name="_attackType">공격 타입 | None, Melee, LongDistance, Magic</param>
    /// <param name="_pos">Effect의 Position 값 | default: new Vector3()</param>
    /// <param name="_rot">Effect의 Quaternion 값(Rotation) | default: new Quaternion()</param>
    /// <param name="_idx">등록된 Effect에 대해 순차 실행될 index값 | default: 0</param>
    /// <param name="_time">지연 재생 시간 | default: 0</param>
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
    /// 타입을 기반으로 등록된 Effect를 재생<br/>
    /// <paramref name="_parent"/>의 Position과 Rotation을 기반으로 생성함<br/>
    /// <br/>
    /// Effect.EActorType <paramref name="_actorType"/> | 사용자 타입 | Player, Monster, Boss, Environment, Custom <br/>
    /// Effect.EActionType <paramref name="_actionType"/> | 행위 타입 | Appear, Idle, Attack, Slash, Hit, Dash, Jump, Run, Etc <br/>
    /// Effect.EAttackType <paramref name="_attackType"/> | 공격 타입 | None, Melee, LongDistance, Magic <br/>
    /// Transform <paramref name="_parent"/> | Effect가 생성될 부모의 위치 <br/>
    /// int <paramref name="_idx"/> | 등록된 Effect에 대해 순차 실행될 index값 | default: 0 <br/>
    /// float <paramref name="_time"/> | 지연 재생 시간 | default: 0<br/>
    /// </summary>
    /// <param name="_actorType">사용자 타입 | Player, Monster, Boss, Environment</param>
    /// <param name="_actionType">행위 타입 | Appear, Idle, Attack, Slash, Hit, Dash, Jump, Run, Etc</param>
    /// <param name="_attackType">공격 타입 | None, Melee, LongDistance, Magic</param>
    /// <param name="_parent">Effect가 생성될 부모의 위치</param>
    /// <param name="_idx">등록된 Effect에 대해 순차 실행될 index값 | default: 0</param>
    /// <param name="_time">지연 재생 시간 | default: 0</param>
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
    /// 등록한 이름을 기반으로 등록된 Effect를 재생<br/>
    /// <paramref name="_parent"/>의 Position과 Rotation을 기반으로 생성함<br/>
    /// <br/>
    /// string <paramref name="_effectName"/> | 등록한 Effect의 이름 <br/>
    /// Transform <paramref name="_parent"/> | Effect가 생성될 부모의 위치 <br/>
    /// int <paramref name="_idx"/> | 등록된 Effect에 대해 순차 실행될 index값 | default: 0 <br/>
    /// float <paramref name="_time"/> | 지연 재생 시간 | default: 0<br/>
    /// </summary>
    /// <param name="_effectName">등록한 Effect의 이름</param>
    /// <param name="_parent">Effect가 생성될 부모의 위치</param>
    /// <param name="_idx">등록된 Effect에 대해 순차 실행될 index값 | default: 0</param>
    /// <param name="_time">지연 재생 시간 | default: 0</param>
    public void Play(string _effectName, Transform _parent, int _idx = 0, float _time = 0f)
    {
        Effect effect = FindEffect(_effectName);
        WaitForPlay(effect, _parent, _idx, _time);
    }
    /// <summary>
    /// 등록한 이름을 기반으로 등록된 Effect를 재생<br/>
    /// <paramref name="_pos"/>는 Effect의 위치값을 설정<br/>
    /// <paramref name="_rot"/>은 Effect의 회전값을 설정<br/> 
    /// <br/>
    /// string <paramref name="_effectName"/> | 등록한 Effect의 이름 <br/>
    /// Vector3 <paramref name="_pos"/> | Effect의 Position 값 <br/>
    /// Quaternion <paramref name="_rot"/> | Effect의 Rotation 값 (Quaternion 기반) <br/>
    /// int <paramref name="_idx"/> | 등록된 Effect에 대해 순차 실행될 index값 | default: 0 <br/>
    /// float <paramref name="_time"/> | 지연 재생 시간 | default: 0<br/>
    /// </summary>
    /// <param name="_effectName">등록한 Effect의 이름</param>
    /// <param name="_pos">Effect의 Position 값 | default: new Vector3()</param>
    /// <param name="_rot">Effect의 Quaternion 값(Rotation) | default: new Quaternion()</param>
    /// <param name="_idx">등록된 Effect에 대해 순차 실행될 index값 | default: 0</param>
    /// <param name="_time">지연 재생 시간 | default: 0</param>
    public void Play(string _effectName, Vector3 _pos, Quaternion _rot, int _idx = 0, float _time = 0f)
    {
        Effect effect = FindEffect(_effectName);
        WaitForPlay(effect, _pos, _rot, _idx, _time);
    }


    /// <summary>
    /// 등록한 Effect의 이름을 통해 Effect를 멈춤<br/>
    /// <br/>
    /// string <paramref name="_effectName"/> | 등록한 Effect의 이름 <br/>
    /// float <paramref name="_time"/> | 정지 지연 시간 | default: 0<br/>
    /// </summary>
    /// <param name="_effectName">등록한 Effect의 이름</param>
    /// <param name="_time">정지 지연 시간 | default: 0</param>
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
