using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4 : MonoBehaviour
{
    public delegate void Stage4Delegate();
    private Stage4Delegate stage4Callback = null;

    [SerializeField] private GameObject bossRoomPos = null;

    private AudioSource audioSource = null;

    float timer;
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            stage4Callback?.Invoke();
            // 일반브금 소리 0으로 서서히 낮추기.
            for (int i = 0; i < SoundManager.Instance.audioSources.Length; ++i)
            {
                if (SoundManager.Instance.audioSources[i].clip.name == "FieldBGM3")
                {
                    audioSource = SoundManager.Instance.audioSources[i];
                    StartCoroutine(OffFieldBGMCoroutine());
                }
            }
            StartCoroutine(SetBossRoom2(_other));
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
        SoundManager.Instance.Stop("FieldBGM3");
    }

    private IEnumerator SetBossRoom2(Collider _other)
    {
        yield return new WaitForSeconds(2f);
        SetBossRoom(_other);
    }

    private void SetBossRoom(Collider _other)
    {
        _other.transform.position = bossRoomPos.transform.position;
        gameObject.SetActive(false);
    }
    public void SetFadeDelegate(Stage4Delegate _stage4Callback)
    {
        stage4Callback = _stage4Callback; 
    }
}
