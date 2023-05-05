using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
    private float timer;
    private AudioSource audioSource = null;
    public void Smash()
    {
        SoundManager.Instance.Play("BossSmash");
    }

    public void Slide()
    {
        SoundManager.Instance.Play("BossSlide");
    }

    public void StopSlide()
    {
        SoundManager.Instance.Stop("BossSlide");
    }

    public void LaserStart()
    {
        SoundManager.Instance.Play("BossLaserStart");
    }

    public void LaserStartStop()
    {
        //SoundManager.Instance.Stop("BossLaserStart");
    }

    public void LaserLoop()
    {
        SoundManager.Instance.Play("BossLaserLoop");
    }

    public void LaserLoopStop()
    {
        SoundManager.Instance.Stop("BossLaserLoop");
    }

    public void Appear()
    {
        SoundManager.Instance.Play("BossStart");
    }

    public void Groggy()
    {
        SoundManager.Instance.Play("BossGroggy");
    }

    public void Died()
    {
        SoundManager.Instance.Play("BossDied");
        for (int i = 0; i < SoundManager.Instance.audioSources.Length; ++i)
        {
            if (SoundManager.Instance.audioSources[i].clip.name == "BossBGM1")
            {
                audioSource = SoundManager.Instance.audioSources[i];
                StartCoroutine(OffFieldBGMCoroutine());
            }
        }
    }

    private IEnumerator OffFieldBGMCoroutine()
    {
        while (timer < 1)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0.04f, 0f, timer);
            yield return null;
        }
        SoundManager.Instance.Stop("BossBGM1");
    }
}
