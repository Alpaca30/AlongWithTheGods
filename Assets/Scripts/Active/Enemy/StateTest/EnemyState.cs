using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    public abstract void CurrentEnemyState();
    public abstract void CurrentEnemyAction();
    public abstract void EnterState();
}
