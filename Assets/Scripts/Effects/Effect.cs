using UnityEngine;

[System.Serializable]
public class Effect
{
    public enum EActorType { Player, Monster, Boss, Environment, Custom };
    public EActorType actorType;

    public enum EActionType { Appear, Dead, Idle, Attack, Slash, Hit, Dash, Jump, Run, Etc };
    public EActionType actionType;

    public enum EAttackType { None, Melee, LongDistance, Magic };
    public EAttackType attackType;

    public string effectName;

    public GameObject[] effects;
}
