using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEvents : MonoBehaviour
{
    private LoadingSceneManager.EScene scene;


    private void Start()
    {
        PortalOpenAnimation();
    }


    public void SetDestination(LoadingSceneManager.EScene _scene)
    {
        scene = _scene;
    }
    public void SetDestination(int _sceneIdx)
    {
        scene = (LoadingSceneManager.EScene)_sceneIdx;
    }

    private void PortalOpenAnimation()
    {
        StartCoroutine(PortalOpenAnimationCoroutine());
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

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            LoadingSceneManager.LoadScene(scene.ToString());
        }
    }
}
