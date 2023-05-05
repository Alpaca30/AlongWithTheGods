using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortalAnimation : MonoBehaviour
{
    private void Start()
    {
        PortalOpenAnimation();
    }


    private void PortalOpenAnimation()
    {
        StartCoroutine(PortalOpenAnimationCoroutine());
        PortalCloseAnimation();
    }
    private void PortalCloseAnimation()
    {
        StartCoroutine(PortalCloseAnimationCoroutine());
    }

    private IEnumerator PortalOpenAnimationCoroutine()
    {
        Vector3 scale = this.transform.localScale;
        float scaleX = scale.x;
        float scaleY = scale.y;

        scale.x = 0f;
        scale.y = 0f;
        this.transform.localScale = scale;

        float t = 0f;
        while (t < 1f)
        {
            scale.x = Mathf.Lerp(0f, scaleX, t);
            scale.y = Mathf.Lerp(0f, scaleY, t);
            this.transform.localScale = scale;
            t += Time.deltaTime;
            yield return null;
        }

        scale.x = scaleX;
        scale.y = scaleY;
        this.transform.localScale = scale;
    }

    private IEnumerator PortalCloseAnimationCoroutine()
    {
        yield return new WaitForSeconds(6f);

        Vector3 scale = this.transform.localScale;
        this.transform.localScale = scale;

        float t = 0f;
        while (t < 1f)
        {
            scale.x = Mathf.Lerp(scale.x, 0f, t);
            scale.y = Mathf.Lerp(scale.y, 0f, t);
            this.transform.localScale = scale;
            t += Time.deltaTime;
            yield return null;
        }

        scale.x = 0f;
        scale.y = 0f;
        this.transform.localScale = scale;
    }
}
