using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeteor : MonoBehaviour
{
    [SerializeField] private Transform[] meteorPos = null;
    [SerializeField] private Meteor meteorPrefab = null;

    private int random;
    private float randomTime;

    public void SpawnMeteor()
    {
        random = Random.Range(0, meteorPos.Length);
        randomTime = Random.Range(0.2f, 0.8f);
        Instantiate(meteorPrefab.gameObject, meteorPos[random]);
        Invoke(nameof(SpawnMeteor), randomTime);
    }

    public void StopMeteor()
    {
        CancelInvoke();
    }
}
