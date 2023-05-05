using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorBox[] doorBox = null;
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            for (int i = 0; i < doorBox.Length; ++i)
            {
                doorBox[i].SetTargetPosition();
            }
        }
    }
}
