using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(TestCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.E)) Time.timeScale = 1.0f;

        if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(TestWalkSound());
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StopAllCoroutines();
            SoundManager.Instance.Play("Laser");
        }

    }

    private IEnumerator TestCoroutine()
    {
        while (true)
        {
            //Debug.Log("1");
            // ui 불타는거나 보스체력깎이는연출이나 뭐 그런것들?

            yield return new WaitForSeconds(1 * (1 * Time.timeScale));
        }
    }

    private void Start()
    {
        //StartCoroutine(TestWalkSound());
        //SoundManager.Instance.Play("BGM");
    }

    private IEnumerator TestWalkSound()
    {
        while (true)
        {
            SoundManager.Instance.Play("TestMonster");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
