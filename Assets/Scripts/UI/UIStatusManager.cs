using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStatusManager : MonoBehaviour
{
    [SerializeField]
    private UIHpController uiHpCtrl = null;
    [SerializeField]
    private UIBossHpController uiBossHpCtrl = null;


    public void Init()
    {
        uiHpCtrl.Init();
        uiBossHpCtrl.Init();
    }


    #region Player HP
    // Player�� ���� Hp�� ����
    public void SetPlayerHp(int _curHp)
    {
        uiHpCtrl.SetPlayerHp(_curHp);
    }
    // Player�� �ִ� Hp�� ����
    public void SetPlayerMaxHp(int _maxHp)
    {
        uiHpCtrl.SetPlayerMaxHp(_maxHp);
    }
    #endregion

    #region Boss HP
    public void SetActiveBossHp(bool _active)
    {
        uiBossHpCtrl.SetActiveBossHp(_active);
    }
    // Boss�� Hp�� ������ ����
    public void SetBossHp(float _curHpRate)
    {
        uiBossHpCtrl.SetBossHp(_curHpRate);
    }
    #endregion
}
