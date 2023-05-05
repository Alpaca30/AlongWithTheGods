using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackAnimation : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider = null;

    public void OnCollider()
    {
        boxCollider.enabled = true;
    }

    public void OffCollider()
    {
        boxCollider.enabled = false;
    }
}
