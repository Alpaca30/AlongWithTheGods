using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    [SerializeField] private BoxCollider leftHand = null;
    [SerializeField] private BoxCollider rightHand = null;
    [SerializeField] private BoxCollider smashBox = null;

    public void OnLeftBoxCollider()
    {
        leftHand.enabled = true;
    }
    public void OnRightBoxCollider()
    {
        rightHand.enabled = true;
    }
    public void OnSmashBox()
    {
        smashBox.enabled = true;
    }
    public void OffCollider()
    {
        leftHand.enabled = false;
        rightHand.enabled = false;
        smashBox.enabled = false;
    }
}
