using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHpController : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas = null;
    [SerializeField]
    private RectTransform parent = null;
    [SerializeField]
    private RectTransform hpPrefab = null;

    private Sprite[] full = null;
    private Sprite[] half = null;

    private List<Image> listHp = new List<Image>();
    private bool isAnimated = true;
    private float sleepTime = 0.05f;

    private int maxHp = 0;
    private int hp = 0;


    private void Awake()
    {
        // Prefab이 설정되어있지 않다면 찾아서 가져옴
        if (hpPrefab == null)
        {
            if (!(hpPrefab = Resources.Load<RectTransform>("Prefabs/UI/P_UI_Hp")))
            {
                Debug.LogWarning("Couldn't Find Prefab.");
                return;
            }
        }
        full = Resources.LoadAll<Sprite>("Sprites/UI/Flame_Full");
        half = Resources.LoadAll<Sprite>("Sprites/UI/Flame_Full_Empty");
        //half = Resources.LoadAll<Sprite>("Sprites/UI/Flame_Half");
    }


    public void Init()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        if (canvases.Length <= 0)
        {
            Debug.LogWarning("Canvas Not Found");
            return;
        }
        else
        {
            for (int i = 0; i < canvases.Length; ++i)
            {
                if (canvases[i].name == "UICanvas")
                    canvas = canvases[i];
            }
        }

        if (parent == null)
        {
            RectTransform[] rts = canvas.GetComponentsInChildren<RectTransform>();
            for (int i = 0; i < rts.Length; ++i)
            {
                if (rts[i].name == "HPGroup")
                {
                    parent = rts[i];
                    break;
                }
            }
        }

        InstantiateHP(maxHp, parent.transform); // HP Bar 생성
        StartSpriteAnimation(); // HP Bar가 생성되면 Animation 실행
    }


    public bool IsAnimated()
    {
        return isAnimated;
    }
    public void IsAnimated(bool _bool)
    {
        isAnimated = _bool;
    }

    public void SetPlayerHp(int _curHp)
    {
        if (_curHp <= 0) _curHp = 0;

        hp = _curHp;
    }
    public void SetPlayerMaxHp(int _maxHp)
    {
        if (_maxHp <= 0) _maxHp = 0;

        maxHp = _maxHp;
    }


    // int _hp | 2로 나누어서 나머지가 1인지 확인. | 1: true, 0: false
    private bool IsHalf(int _hp)
    {
        return _hp % 2 == 1;
    }

    // Image _img | Alpha값을 변경할 대상
    // float _a | Color의 Alpha값
    private void ChangeAlpha(Image _img, float _a)
    {
        Color c = _img.color;
        c.a = _a;
        _img.color = c;
    }


    // HP 이미지 생성
    // int _maxHp | 최대 HP
    // Transform _parent | Hierarchy상 부모가 될 대상
    private void InstantiateHP(int _maxHp, Transform _parent)
    {
        if (_maxHp <= 0 || _parent == null) return;

        float w = 50f;
        float h = -50f;
        float wOffset = 6f;

        int cnt = Mathf.CeilToInt(_maxHp * 0.5f);

        for (int i = 0; i < cnt; ++i)
        {
            RectTransform rt = Instantiate(hpPrefab);

            rt.SetParent(_parent);
            float sizeX = rt.sizeDelta.x;
            rt.anchoredPosition = new Vector2(w + ((sizeX + wOffset) * i), h);
            rt.localScale = new Vector3(1f, 1f); // 해상도에 따른 스케일 조정 문제 때문에 스케일 고정으로 먹임 (임시방편)

            Image img = null;
            if (rt.TryGetComponent<Image>(out img))
                listHp.Add(img);
        }
    }


    #region Sprite Animation Coroutine
    private void StartSpriteAnimation()
    {
        StartCoroutine("SpriteAnimationCoroutine");
    }

    private IEnumerator SpriteAnimationCoroutine()
    {
        int idx = 0;
        while (true)
        {
            yield return new WaitUntil(() => IsAnimated()); // isAnimated가 true일 때까지 동작하지 않음
            idx %= full.Length; // sprite index가 한 사이클을 돌았다면 초기화

            int hpCnt = Mathf.CeilToInt(hp * 0.5f);
            int cnt = listHp.Count;
            for (int i = 0; i < cnt; ++i)
            {
                if (hpCnt > i)
                {
                    ChangeAlpha(listHp[i], 1f);
                    if (i >= hpCnt - 1 && IsHalf(hp))
                        listHp[i].sprite = half[idx];
                    else
                        listHp[i].sprite = full[idx];
                }
                else
                    ChangeAlpha(listHp[i], 0f);
            }
            yield return new WaitForSeconds(sleepTime);

            ++idx;
        }
    }
    #endregion
}