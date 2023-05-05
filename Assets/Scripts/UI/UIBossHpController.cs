using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHpController : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar = null;
    //[SerializeField]
    //private RectTransform hpBackSide = null;

    private Image gazer = null;
    private float gaze;


    private void Awake()
    {
        
    }


    public void Init()
    {

    }

    #region Getter/Setter
    private float GetBossHp()
    {
        return hpBar.value;
    }
    private float GetGazer()
    {
        return gazer.fillAmount;
    }
    #endregion


    public void SetActiveBossHp(bool _active)
    {
        hpBar.gameObject.SetActive(_active);
    }

    public void SetBossHp(float _curHpRate)
    {
        if (_curHpRate < 0f) _curHpRate = 0f;
        else if (_curHpRate > 1f) _curHpRate = 1f;

        gaze = hpBar.value;
        hpBar.value = _curHpRate;
    }


    private void BossHpHitAnimation()
    {
        StartCoroutine(BossHpHitAnimationCoroutine());
    }

    private IEnumerator BossHpHitAnimationCoroutine()
    {
        while(GetBossHp() <= GetGazer())
        {
            gazer.fillAmount = Mathf.Lerp(gaze, GetBossHp(), 1f);
            yield return null;
        }
    }
}
