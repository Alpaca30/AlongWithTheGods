using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackgroundAnimator : MonoBehaviour
{
    private enum EAnimationType { Single, SingleLoop, Loop };

    [SerializeField]
    private Image image = null;
    [SerializeField]
    private Sprite[] sprites = null;
    [SerializeField]
    private float delayTime = 0.1f;

    private bool isReverse = false;


    private void Awake()
    {
        if (image == null) return;
        image.gameObject.SetActive(false);

        if (sprites == null)
        {
            sprites = Resources.LoadAll<Sprite>("Sprites/UI/Backgrounds/Main_Background_Character_Sheet");
        }
    }

    private void Start()
    {
        if (image == null) return;

        image.gameObject.SetActive(true);
        StartCoroutine("BackgroundCharacterCoroutine");
    }


    private Sprite SpriteAnimationIndexer(Sprite[] _sprites, EAnimationType _type, ref int _idx)
    {
        if (_sprites == null) return null;

        int len = _sprites.Length;
        if (_type == EAnimationType.Loop)
        {
            if (isReverse)
                --_idx;
            else
                ++_idx;

            if(_idx <= 0)
                isReverse = false;
            else if (_idx >= len - 1)
                isReverse = true;
        }
        else
        {
            ++_idx;

            if (_type == EAnimationType.SingleLoop)
                _idx %= len;
        }

        return _sprites[_idx];
    }

    private IEnumerator BackgroundCharacterCoroutine()
    {
        int idx = 0;
        while (true)
        {
            image.sprite = SpriteAnimationIndexer(sprites, EAnimationType.Loop, ref idx);
            yield return new WaitForSeconds(delayTime);
        }
    }
}
