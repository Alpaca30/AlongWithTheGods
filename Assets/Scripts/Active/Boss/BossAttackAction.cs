using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackAction : MonoBehaviour
{
    public delegate void BossLaserDelegate();
    private BossLaserDelegate bossLaserCallback = null;

    public delegate void StoryActionDelegate();
    private StoryActionDelegate storyActionCallback = null;
    private StoryActionDelegate bossDeadCallback = null;

    [SerializeField] private BossLaser bossLaser = null;

    public bool action { get; set; }

    private void Start()
    {
        bossLaser.SetLaserOffDelegate(SetCallback);
        action = false;
    }

    public void SetAction()
    {
        action = !action;
    }

    public void SetActionFalse()
    {
        action = false;
    }

    public void SetLaserActive()
    {
        int random = Random.Range(0, 2);

        bossLaser.gameObject.SetActive(true);
        bossLaser.isRotating = true;
        if (random == 0) bossLaser.rotateDir = 1;
        else if (random == 1) bossLaser.rotateDir = -1;
    }

    public void SetLaserOff()
    {
        bossLaser.gameObject.SetActive(false);
        bossLaser.isRotating = false;
        bossLaser.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        bossLaser.timer = 0f;
    }

    private void SetCallback()
    {
        bossLaserCallback?.Invoke();
    }

    public void SetBossLaserDelegate(BossLaserDelegate _bossLaserCallback)
    {
        bossLaserCallback = _bossLaserCallback;
    }

    public void ChangeStoryAction()
    {
        storyActionCallback?.Invoke();
    }

    public void BossDead()
    {
        bossDeadCallback?.Invoke();
    }

    public void ChangeStoryActionDelegate(StoryActionDelegate _storyActionCallback, StoryActionDelegate _bossDeadCallback)
    {
        storyActionCallback = _storyActionCallback;
        bossDeadCallback = _bossDeadCallback;
    }
}
