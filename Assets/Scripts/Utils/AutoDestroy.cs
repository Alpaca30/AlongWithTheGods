using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTime = 0f;


    private void Update()
    {
        if (gameObject != null)
            Destroy(gameObject, destroyTime);
    }
}
