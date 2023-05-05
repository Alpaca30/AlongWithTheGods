using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public delegate void EnemyManagerDelegate();
    private EnemyManagerDelegate enemyManagerCallback = null;
    
    public EnemySpawnManager enemySpawnManager = null;

    public void Init()
    {
        enemySpawnManager.SetSpawnDelegate(InvokeCallback);
    }

    private void InvokeCallback()
    {
        enemyManagerCallback?.Invoke();
    }

    public void SetEnemyManagerDelegate(EnemyManagerDelegate _enemyManagerCallback)
    {
        enemyManagerCallback = _enemyManagerCallback;
    }
}
