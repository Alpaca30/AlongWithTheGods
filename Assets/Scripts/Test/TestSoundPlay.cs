using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundPlay : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(WalkSoundCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestSoundTest.Instance.Play("Dash");
        }
    }

    private IEnumerator WalkSoundCoroutine()
    {
        while (true)
        {
            TestSoundTest.Instance.Play("Walk");
        yield return new WaitForSeconds(0.5f);
        }
    }
}
