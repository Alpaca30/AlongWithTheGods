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

    // OptionUI�� Options
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


    // �������� UI�� ���/�����ϴ� ����
    private void Awake()
    {
        // Canvas�� ���ٸ� ����
        CheckAndGenerateCanvas();

        // OptionUI�� ���� ���ߴٸ� ����
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

        //// ���� �����ͼ� UI�� ���
        //UpdateOptionsValue();
    }


    // Settings�κ��� �������� �����ͼ� UI�� �ݿ�
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
    // UISettingEvent�κ��� �������� �޾Ƽ� UI�� �ݿ�
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
        �����ִ��� Ȯ���Ͽ� ���������� �ݱ⸦ ����.
        ���������� �޴�â�� �����ִٸ� �޴�â�� ����.
    */
    private void OpenAndCloseUIProcess()
    {
        // InGame���� Esc��ư�� ������ ��,
        // ������ �����ִ� ������� UI�� ������ ����
        if (IsPromptUIActive() == true) // Ȯ��â�� �����ִٸ� �ݱ�
            ActivePromptUI(false);
        else if (IsOptionUIActive() == true) // �ɼ�â�� �����ִٸ� �ݱ�
            ActiveOptionUI(false);
        else if (IsMenuUIActive() == true) // �޴�â�� �����ִٸ� �ݱ�
            ActiveMenuUI(false);
        else if (IsMenuUIActive() == false) // �޴�â�� �����ִٸ� ����
            ActiveMenuUI(true);
    }

    /*
        MenuUI(�޴�â)�� Ȱ��ȭ �������� Ȯ��
        return | bool
    */
    private bool IsMenuUIActive()
    {
        if (menuUI == null) return false;

        return menuUI.gameObject.activeSelf;
    }
    /*
        OptionUI(����â)�� Ȱ��ȭ �������� Ȯ��
        return | bool
    */
    private bool IsOptionUIActive()
    {
        if (optionUI == null) return false;

        return optionUI.gameObject.activeSelf;
    }
    /*
        PromptUI(Ȯ��â)�� Ȱ��ȭ �������� Ȯ��
        return | bool
    */
    private bool IsPromptUIActive()
    {
        if (promptUI == null) return false;

        return promptUI.gameObject.activeSelf;
    }

    /*
        MenuUI(�޴�â) Ȱ��ȭ/��Ȱ��ȭ
        bool _active | Ȱ��ȭ/��Ȱ��ȭ
    */
    private void ActiveMenuUI(bool _active)
    {
        if (menuUI == null) return;

        menuUI.gameObject.SetActive(_active);
    }
    /*
        OptionUI(����â) Ȱ��ȭ/��Ȱ��ȭ
        bool _active | Ȱ��ȭ/��Ȱ��ȭ
    */
    private void ActiveOptionUI(bool _active)
    {
        if (optionUI == null) return;

        optionUI.gameObject.SetActive(_active);
    }
    /*
        PromptUI(Ȯ��â) Ȱ��ȭ/��Ȱ��ȭ
        bool _active | Ȱ��ȭ/��Ȱ��ȭ
    */
    private void ActivePromptUI(bool _active)
    {
        if (promptUI == null) return;

        promptUI.gameObject.SetActive(_active);
    }


    #region Generate UI
    /*
        Canva�� null���� Ȯ���Ͽ� null�̸� Canvas�� ����
    */
    private void CheckAndGenerateCanvas()
    {
        // ĵ������ ���ٸ� ����
        if (canvas == null)
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            if (canvases.Length <= 0) // Canvas�� ���ٸ� ����
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
        OptionUI�� null���� Ȯ���Ͽ� null�̸� Prefab�� �����ͼ� �����ϰ� OptionUI�� �� UI���� ������
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
        OptionUI�κ��� �ʿ��� UI���� ������
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
        string name | ������ Canvas�� �̸� ����
        out | Canvas _canvas | ������ Canvas�� ���� ����
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
        string _name | ������ Image�� �̸� ����
        ref | Image _img | ������ Image�� ���� Image ����
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
        string _name | ������ Image�� �̸� ����
        string _path | Image�� ����� Sprite�� ���(���� ���: Resources) | Resources/(_path)
        ref | Image _img | ������ Image�� ���� Image ����
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
        string _name | ������ Image�� �̸� ����
        Sprite _sprite | Image�� ����� Sprite
        ref | Image _img | ������ Image�� ���� Image ����
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
