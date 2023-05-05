using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutManager : MonoBehaviour
{
    Color c;
    private void Awake()
    {
        c = gameObject.GetComponent<Image>().color;
        c = Color.black;
        c.a = 0f;
        gameObject.GetComponent<Image>().color = c;
    }

    public void StartFade()
    {
        StartCoroutine(FadeInOutStart());
    }

    public IEnumerator FadeInOutStart()
    {
        for (float f = 0f; f < 1; f += 0.01f)
        {
            c.a = f;
            gameObject.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2f);
        for (float f = 1f; f > 0; f -= 0.01f)
        {
            c.a = f;
            gameObject.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
