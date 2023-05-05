using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooltime : MonoBehaviour
{
    private Image img = null;


    private void Awake()
    {
        if(TryGetComponent<Image>(out img) == false)
        {
#if UNITY_EDITOR
            //Debug.Log("[UICooltime] Fail to GetComponent Image.");
#endif
        }
    }


    public Image GetImage()
    {
        return img;
    }

    // ��Ÿ���� ��ġ�� ����
    // float _amount | Cooltime ���൵ | 0.0f ~ 1.0f
    public void SetCooltimeProgress(float _amount)
    {
        if (_amount > 1f) _amount = 1f;
        else if (_amount < 0f) _amount = 0f;

        img.fillAmount = _amount;
    }
}
