using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState : MonoBehaviour
{
    public abstract void CurrentBossState();
    public abstract void CurrentBossUpdateAction();
    public abstract void CurrentBossStartAction();
}
