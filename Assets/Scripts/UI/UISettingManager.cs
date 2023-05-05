using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UISettingManager : MonoBehaviour
{
    // Get Settings Value Callback
    public delegate float GetMusicVolumeCallback();
    public delegate float GetSoundEffectsVolumeCallback();
    public delegate int GetFullScreenModeOptionCallback();
    public delegate int GetResolutionOptionCallback();
    public delegate int GetQualityCallback();
    private GetMusicVolumeCallback getMusicVolume = null;
    private GetSoundEffectsVolumeCallback getSoundEffectsVolume = null;
    private GetFullScreenModeOptionCallback getFullScreenModeOption = null;
    private GetResolutionOptionCallback getResolutionOption = null;
    private GetQualityCallback getQuality = null;

    [SerializeField]
    private UISettingEvents events = null;

    [SerializeField]
    private Canvas canvas = null;
    [SerializeField]
    private RectTransform menuUI = null; // Object
    [SerializeField]
    private RectTransform optionUI = null; // Object
    [SerializeField]
    private RectTransform promptUI = null; // Object

    // OptionUI의 Options
    [SerializeField]
    private Slider musicSlider = null;
    [SerializeField]
    private Slider soundEffectsSlider = null;
    [SerializeField]
    private TMP_Dropdown fullScreenModeDropdown = null;
    [SerializeField]
    private TMP_Dropdown resolutionDropdown = null;
    [SerializeField]
    private TMP_Dropdown qualityDropdown = null;

    private Scene scene;


    // 설정값을 UI로 출력/관리하는 역할
    private void Awake()
    {
        // Canvas가 없다면 생성
        CheckAndGenerateCanvas();

        // OptionUI를 받지 못했다면 생성
        CheckAndGenerateOptionUI();
    }

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenAndCloseUIProcess();
    }


    public void Init(
        GetMusicVolumeCallback _getMusicVolumeCallback,
        GetSoundEffectsVolumeCallback _getSoundEffectsVolumeCallback,
        GetFullScreenModeOptionCallback _getFullScreenModeOptionCallback,
        GetResolutionOptionCallback _getResolutionOptionCallback,
        GetQualityCallback _getQualityCallback,

        // UISettingEvents //
        UISettingEvents.UpdateSettingsINICallback _updateINICallback,

        UISettingEvents.GetMusicVolumeCallback _getMusicVolume,
        UISettingEvents.GetSoundEffectsVolumeCallback _getSoundEffectsVolume,
        UISettingEvents.GetFullScreenModeOptionCallback _getFullScreenModeOption,
        UISettingEvents.GetResolutionOptionCallback _getResolutionOption,
        UISettingEvents.GetQualityCallback _getQuality,

        UISettingEvents.OnChangedMusicVolumeCallback _musicVolumeCallback,
        UISettingEvents.OnChangedSoundEffectsVolumeCallback _soundEffectsVolumeCallback,
        UISettingEvents.OnChangedFullScreenModeCallback _fullScreenModeCallback,
        UISettingEvents.OnChangedResolutionCallback _resolutionCallback,
        UISettingEvents.OnChangedQualityCallback _qualityCallback
        )
    {
        // UISettingManager //
        getMusicVolume = _getMusicVolumeCallback;
        getSoundEffectsVolume = _getSoundEffectsVolumeCallback;
        getFullScreenModeOption = _getFullScreenModeOptionCallback;
        getResolutionOption = _getResolutionOptionCallback;
        getQuality = _getQualityCallback;

        // UISettingEvents //
        events.Init(
            _updateINICallback,

            UpdateOptionsValue,

            _getMusicVolume,
            _getSoundEffectsVolume,
            _getFullScreenModeOption,
            _getResolutionOption,
            _getQuality,

            _musicVolumeCallback,
            _soundEffectsVolumeCallback,
            _fullScreenModeCallback,
            _resolutionCallback,
            _qualityCallback
            );

        //// 값을 가져와서 UI에 출력
        //UpdateOptionsValue();
    }


    // Settings로부터 설정값을 가져와서 UI에 반영
    public void UpdateOptionsValue()
    {
        //musicSlider.value = Settings.Instance.GetMusicVolume();
        //soundEffectsSlider.value = Settings.Instance.GetSoundEffectsVolume();
        //fullScreenModeDropdown.value = Settings.Instance.GetFullScreenModeOption();
        //resolutionDropdown.value = Settings.Instance.GetResolutionOption();
        //qualityDropdown.value = Settings.Instance.GetQuality();

        musicSlider.value               = (float)getMusicVolume?.Invoke();
        soundEffectsSlider.value        = (float)getSoundEffectsVolume?.Invoke();
        fullScreenModeDropdown.value    = (int)getFullScreenModeOption?.Invoke();
        resolutionDropdown.value        = (int)getResolutionOption?.Invoke();
        qualityDropdown.value           = (int)getQuality?.Invoke();
    }
    // UISettingEvent로부터 설정값을 받아서 UI에 반영
    public void UpdateOptionsValue(
        float _musicVolume,
        float _soundEffectsVolume,
        int _fullScreenMode,
        int _resolution,
        int _quality
        )
    {
        musicSlider.value               = _musicVolume;
        soundEffectsSlider.value        = _soundEffectsVolume;
        fullScreenModeDropdown.value    = _fullScreenMode;
        resolutionDropdown.value        = _resolution;
        qualityDropdown.value           = _quality;
    }
    /*
        열려있는지 확인하여 순차적으로 닫기를 실행.
        최종적으로 메뉴창이 닫혀있다면 메뉴창을 열음.
    */
    private void OpenAndCloseUIProcess()
    {
        // InGame에서 Esc버튼을 눌렀을 때,
        // 상위에 켜져있는 순서대로 UI를 끄도록 만듬
        if (IsPromptUIActive() == true) // 확인창이 열려있다면 닫기
            ActivePromptUI(false);
        else if (IsOptionUIActive() == true) // 옵션창이 열려있다면 닫기
            ActiveOptionUI(false);
        else if (IsMenuUIActive() == true) // 메뉴창이 열려있다면 닫기
            ActiveMenuUI(false);
        else if (IsMenuUIActive() == false) // 메뉴창이 닫혀있다면 열기
            ActiveMenuUI(true);
    }

    /*
        MenuUI(메뉴창)이 활성화 상태인지 확인
        return | bool
    */
    private bool IsMenuUIActive()
    {
        if (menuUI == null) return false;

        return menuUI.gameObject.activeSelf;
    }
    /*
        OptionUI(설정창)이 활성화 상태인지 확인
        return | bool
    */
    private bool IsOptionUIActive()
    {
        if (optionUI == null) return false;

        return optionUI.gameObject.activeSelf;
    }
    /*
        PromptUI(확인창)이 활성화 상태인지 확인
        return | bool
    */
    private bool IsPromptUIActive()
    {
        if (promptUI == null) return false;

        return promptUI.gameObject.activeSelf;
    }

    /*
        MenuUI(메뉴창) 활성화/비활성화
        bool _active | 활성화/비활성화
    */
    private void ActiveMenuUI(bool _active)
    {
        if (menuUI == null) return;

        menuUI.gameObject.SetActive(_active);
    }
    /*
        OptionUI(설정창) 활성화/비활성화
        bool _active | 활성화/비활성화
    */
    private void ActiveOptionUI(bool _active)
    {
        if (optionUI == null) return;

        optionUI.gameObject.SetActive(_active);
    }
    /*
        PromptUI(확인창) 활성화/비활성화
        bool _active | 활성화/비활성화
    */
    private void ActivePromptUI(bool _active)
    {
        if (promptUI == null) return;

        promptUI.gameObject.SetActive(_active);
    }


    #region Generate UI
    /*
        Canva가 null인지 확인하여 null이면 Canvas를 생성
    */
    private void CheckAndGenerateCanvas()
    {
        // 캔버스가 없다면 생성
        if (canvas == null)
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            if (canvases.Length <= 0) // Canvas가 없다면 생성
            {
                GenerateCanvas("MainCanvas", out canvas);
            }
            else
            {
                for (int i = 0; i < canvases.Length; ++i)
                {
                    if (canvases[i].name == "MainCanvas")
                        canvas = canvases[i];
                }

                if (canvas == null)
                    GenerateCanvas("MainCanvas", out canvas);
            }
        }
    }

    /*
        OptionUI가 null인지 확인하여 null이면 Prefab을 가져와서 생성하고 OptionUI의 각 UI들을 가져옴
    */
    private void CheckAndGenerateOptionUI()
    {
        if (optionUI == null)
        {
            RectTransform go = Resources.Load<RectTransform>("Prefabs/UI/P_UI_OptionUI");
            if (go == null)
            {
                Debug.LogWarningFormat("Couldn't Load UI Prefab.");
            }
            else
            {
                optionUI = Instantiate(go, canvas.transform);

                GetOptionUIs();
            }
        }
        else
        {
            if (musicSlider == null ||
                soundEffectsSlider == null ||
                fullScreenModeDropdown == null ||
                resolutionDropdown == null ||
                qualityDropdown == null)
            {
                GetOptionUIs();
            }
        }
    }

    /*
        OptionUI로부터 필요한 UI들을 가져옴
    */
    private void GetOptionUIs()
    {
        Slider[] sliders = optionUI.GetComponentsInChildren<Slider>();
        for (int i = 0; i < sliders.Length; ++i)
        {
            if (sliders[i].name == "Slider_Music")
                musicSlider = sliders[i];
            else if (sliders[i].name == "Slider_SFX")
                soundEffectsSlider = sliders[i];
        }

        TMP_Dropdown[] dropdowns = optionUI.GetComponentsInChildren<TMP_Dropdown>();
        for (int i = 0; i < dropdowns.Length; ++i)
        {
            if (dropdowns[i].name == "Dropdown_Screen")
                fullScreenModeDropdown = dropdowns[i];
            else if (dropdowns[i].name == "Dropdown_Resolution")
                resolutionDropdown = dropdowns[i];
            else if (dropdowns[i].name == "Dropdown_Quality")
                qualityDropdown = dropdowns[i];
        }
    }

    /*
        string name | 생성할 Canvas의 이름 설정
        out | Canvas _canvas | 생성한 Canvas를 담을 변수
    */
    private void GenerateCanvas(string _name, out Canvas _canvas)
    {
        GameObject go = new GameObject();
        go.name = _name;
        _canvas = go.AddComponent<Canvas>();
        CanvasScaler scaler = go.AddComponent<CanvasScaler>();
        go.AddComponent<GraphicRaycaster>();

        // Canvas Scaler Setting
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        Resolution res = Screen.currentResolution;
        scaler.referenceResolution = new Vector2(res.width, res.height);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
    }

    /*
        string _name | 생성할 Image의 이름 설정
        ref | Image _img | 생성한 Image를 담을 Image 변수
    */
    private void GenerateImage(string _name, ref Image _img)
    {
        if (_name == null || _name == string.Empty || _name == "")
            _name = "Image";

        GameObject go = new GameObject();
        go.name = _name;
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(1f, 1f);
        _img = go.AddComponent<Image>();
        _img.raycastTarget = false;
    }

    /*
        string _name | 생성할 Image의 이름 설정
        string _path | Image에 사용할 Sprite의 경로(시작 경로: Resources) | Resources/(_path)
        ref | Image _img | 생성한 Image를 담을 Image 변수
    */
    private void GenerateImage(string _name, string _path, ref Image _img)
    {
        if (_img == null)
            GenerateImage(_name, ref _img);

        Sprite sprite = Resources.Load<Sprite>(_path);
        if (sprite == null) return;

        _img.sprite = sprite;
    }

    /*
        string _name | 생성할 Image의 이름 설정
        Sprite _sprite | Image에 사용할 Sprite
        ref | Image _img | 생성한 Image를 담을 Image 변수
    */
    private void GenerateImage(string _name, Sprite _sprite, ref Image _img)
    {
        if (_img == null)
            GenerateImage(_name, ref _img);

        if (_sprite == null) return;

        _img.sprite = _sprite;
    }
    #endregion
}
