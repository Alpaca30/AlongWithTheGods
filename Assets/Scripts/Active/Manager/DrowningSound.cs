using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrowningSound : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    SoundManager.Instance.PlayOneShot("Drowning1");
                    break;
                case 1:
                    SoundManager.Instance.PlayOneShot("Drowning2");
                    break;
            }
        }
    }
}
