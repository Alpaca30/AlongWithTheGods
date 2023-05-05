using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private UISettingManager uiSettingMg = null;

    [SerializeField]
    private Slider musicSlider = null;
    [SerializeField]
    private Slider soundEffectsSlider = null;


    private float musicVolume = 1f; // ����
    private float soundEffectsVolume = 1f; // SFX
    private FullScreenMode fullScreenMode; // â ���
    private int fullScreenModeOption = 0; // â ��� Dropdown ��
    private Resolution currentResolution; // �ػ�
    private int resolutionOption = 0; // �ػ� Dropdown ��
    private int quality = 0; // ǰ��


    private void Start()
    {
        Init();
        // ���� ���� �ִ��� Ȯ����
        // �ִٸ� ���� ������ �о ó��
        if (GetSettingsINIValues() == false)
        {
            // ���ٸ� �⺻ �������� ó���ϰ� �������� ����
            SettingInitValue();
        }
        uiSettingMg.UpdateOptionsValue();
    }

    /*
        // UISettingManager //
        UISettingManager.GetMusicVolumeCallback
        UISettingManager.GetSoundEffectsVolumeCallback
        UISettingManager.GetFullScreenModeOptionCallback
        UISettingManager.GetResolutionOptionCallback
        UISettingManager.GetQualityCallback

        // UISettingEvents //
        UISettingEvents.UpdateSettingUICallback

        UISettingEvents.GetMusicVolumeCallback
        UISettingEvents.GetSoundEffectsVolumeCallback
        UISettingEvents.GetFullScreenModeCallback
        UISettingEvents.GetFullScreenModeOptionCallback
        UISettingEvents.GetResolutionCallback
        UISettingEvents.GetResolutionOptionCallback
        UISettingEvents.GetQualityCallback

        UISettingEvents.OnChangedMusicVolumeCallback
        UISettingEvents.OnChangedSoundEffectsVolumeCallback
        UISettingEvents.OnChangedFullScreenModeCallback
        UISettingEvents.OnChangedResolutionCallback
        UISettingEvents.OnChangedQualityCallback
    */
    public void Init()
    {
        uiSettingMg.Init(
            // UISettingManager //
            GetMusicVolume,
            GetSoundEffectsVolume,
            GetFullScreenModeOption,
            GetResolutionOption,
            GetQuality,

            // UISettingEvents //
            UpdateSettingINI,

            GetMusicVolume,
            GetSoundEffectsVolume,
            GetFullScreenModeOption,
            GetResolutionOption,
            GetQuality,

            SetMusicVolume,
            SetSoundEffectsVolume,
            SetFullScreenMode,
            SetResolution,
            SetQuality
            );
    }


    public void UpdateSettingINI()
    {
        FileManager fm = new FileManager();
        fm.WriteIniData(
            musicVolume, 
            soundEffectsVolume, 
            fullScreenModeOption, 
            currentResolution.width, 
            currentResolution.height, 
            quality
            );
    }

    private bool GetSettingsINIValues()
    {
        FileManager fm = new FileManager();
        Dictionary<string, Dictionary<string, string>> sections = fm.ReadIniData();

        if (sections != null)
        {
            if (sections.ContainsKey("audio") && sections.ContainsKey("graphics"))
            {
                string musicVolumeString = sections["audio"]["musicVolume"];
                string soundEffectsVolumeString = sections["audio"]["soundEffectsVolume"];
                string fullscreenString = sections["graphics"]["fullscreen"];
                string widthString = sections["graphics"]["width"];
                string heightString = sections["graphics"]["height"];
                string qualityString = sections["graphics"]["quality"];

        #if UNITY_EDITOR
                //Debug.LogFormat("Music Volume: {0}, Sound Effects Volume: {1}, Fullscreen: {2}, Resolution: {3}x{4}, Quality: {5}",
                //    musicVolumeString, soundEffectsVolumeString, fullscreenString, widthString, heightString, qualityString);
        #endif
                int width;
                int height;

                float.TryParse(musicVolumeString, out musicVolume);
                float.TryParse(soundEffectsVolumeString, out soundEffectsVolume);
                int.TryParse(fullscreenString, out fullScreenModeOption);
                int.TryParse(widthString, out width);
                int.TryParse(heightString, out height);
                int.TryParse(qualityString, out quality);

                SetMusicVolume(musicVolume);
                SetSoundEffectsVolume(soundEffectsVolume);
                SetFullScreenMode(ChangeToFullScreenMode(fullScreenModeOption));
                SetResolution(width, height);
                SetQuality(quality);

                // ���� ���� �����ߴٸ� UI�󿡼� �ݿ��ϵ��� ����
                uiSettingMg.UpdateOptionsValue();

                // �������� ��� �� �о�Դٸ� true ��ȯ
                return true;
            }
        }

        // �������� ���� ���ߴٸ� false ��ȯ
        return false;
    }

    // ���� ������ ���� �޾ƿ��� ���ߴٸ� �ʱⰪ���� ����
    private void SettingInitValue()
    {
        musicVolume = 1f;
        soundEffectsVolume = 1f;
        fullScreenModeOption = 0;
        int width = 1280, height = 720;
        quality = 0;

        SetMusicVolume(musicVolume);
        SetSoundEffectsVolume(soundEffectsVolume);
        SetFullScreenMode(fullScreenModeOption);
        SetResolution(width, height);
        SetQuality(quality);

        FileManager fm = new FileManager();
        fm.WriteIniData(musicVolume, soundEffectsVolume, fullScreenModeOption, width, height, quality);
    }


    #region Music Volume
    // Music�� ���� �������� ������
    // return | float
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    // float _vol | Music�� ������ ������ | 0.0001f ~ 1f
    public void SetMusicVolume(float _vol)
    {
        if (_vol <= 0f) _vol = 0.0001f;
        else if (_vol >= 1f) _vol = 1f;

        musicVolume = _vol;
        musicSlider.value = _vol;
    }
    #endregion Music Volume


    #region Sound Effects Volume
    // Sound Effects�� ���� �������� ������
    // return | float
    public float GetSoundEffectsVolume()
    {
        return soundEffectsVolume;
    }

    // float _vol | Sound Effects�� ������ ������ | 0.0001f ~ 1f
    public void SetSoundEffectsVolume(float _vol)
    {
        if (_vol <= 0f) _vol = 0.0001f;
        else if (_vol >= 1f) _vol = 1f;

        soundEffectsVolume = _vol;
        soundEffectsSlider.value = _vol;
    }
    #endregion Sound Effects Volume


    #region FullScreenMode
    // ���� FullScreenMode ���� ���� ������
    public FullScreenMode GetFullScreenMode()
    {
        return Screen.fullScreenMode;
    }
    public int GetFullScreenModeOption()
    {
        return fullScreenModeOption;
    }
    // ���� ��üȭ������ ������
    public bool IsFullScreen()
    {
        return Screen.fullScreen;
    }

    // ��üȭ�� ���� ��üȭ�� ���θ� ����
    public void SetFullScreenMode()
    {
        Screen.fullScreenMode = fullScreenMode;
        Screen.fullScreen = (int)fullScreenMode == 3 ? false : true;
    }
    // bool _isFull | ��üȭ������ ����
    public void SetFullScreenMode(bool _isFull)
    {
        fullScreenModeOption = _isFull ? 0 : 1;
        fullScreenMode = ChangeToFullScreenMode(_isFull);
        SetFullScreenMode();
    }
    // int _mode | FullScreenMode�� int�� ��. â ��� ���� | 0: FullScreen(Windows), 1: FullScreen, 2: FullScreen(OSX), 3: Windowed | default: FullScreen
    public void SetFullScreenMode(int _mode)
    {
        fullScreenModeOption = _mode;
        fullScreenMode = ChangeToFullScreenMode(_mode);
        SetFullScreenMode();
    }
    // FullScreenMode _mode | â ��� ���� | 0: FullScreen(Windows), 1: FullScreen, 2: FullScreen(OSX), 3: Windowed
    public void SetFullScreenMode(FullScreenMode _mode)
    {
        fullScreenModeOption = ChangeFullScreenModeToOption(_mode);
        fullScreenMode = _mode;
        SetFullScreenMode();
    }
    #endregion FullScreenMode


    #region Resolution
    // ���� �ػ� �������� ������
    public Resolution GetResolution()
    {
        return Screen.currentResolution;
    }
    public int GetResolutionOption()
    {
        return resolutionOption;
    }

    // �ػ󵵸� ����
    public void SetResolution()
    {
        Screen.SetResolution(currentResolution.width, currentResolution.height, fullScreenMode);
    }
    // int _width | �ػ� width | default: 640
    // int _height | �ػ� height | default: 360
    public void SetResolution(int _width, int _height)
    {
        if (_width <= 640) _width = 640;
        if (_height <= 360) _height = 360;

        Resolution resolution = new Resolution();
        resolution.width = _width;
        resolution.height = _height;
        currentResolution = resolution;
        resolutionOption = ChangeResolutionToOption(currentResolution);
        SetResolution();
    }
    // int _option | �ػ� Dropdown ���� �� | default: 0
    public void SetResolution(int _option)
    {
        currentResolution = ChangeToResolution(_option);
        resolutionOption = _option;
        SetResolution();
    }
    // Resolution _resolution | �ػ� ����ü
    public void SetResolution(Resolution _resolution)
    {
        currentResolution = _resolution;
        resolutionOption = ChangeResolutionToOption(_resolution);
        SetResolution();
    }
    // UIOptions.EResolution _resolution | �ػ� Dropdown �� | default: _640x360
    public void SetResolution(UIOptions.EResolution _resolution)
    {
        currentResolution = ChangeToResolution(_resolution);
        resolutionOption = (int)_resolution;
        SetResolution();
    }
    #endregion Resolution


    #region Quality
    // ���� ǰ�� �ܰ踦 ������
    public int GetQuality()
    {
        return QualitySettings.GetQualityLevel();
    }

    // ������ ǰ�� ����
    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(quality);
    }
    // int _lv | ǰ�� ���� �� | default: 0
    public void SetQuality(int _lv)
    {
        if (_lv < 0 || _lv >= (int)UIOptions.EQuality.Count) _lv = 0;
            
        quality = _lv;
        SetQuality();
    }
    #endregion Quality


    #region ��ȯ�� �Լ�
    /*
        â ��� �������� FullScreenMode(enum)���� ��ȯ
        bool _isFull | ��üȭ������ ����
        return | FullScreenMode | default: FullScreenMode.FullScreenWindow
    */
    private FullScreenMode ChangeToFullScreenMode(bool _isFull)
    {
        if (_isFull) // ��üȭ��
        {
            RuntimePlatform platform = Application.platform;
            switch(platform)
            {
                case RuntimePlatform.WindowsPlayer:
                    return FullScreenMode.ExclusiveFullScreen;
                case RuntimePlatform.OSXPlayer:
                    return FullScreenMode.MaximizedWindow;
                default:
                    return FullScreenMode.FullScreenWindow;
            }
        }
        else // â���
        {
            return FullScreenMode.Windowed;
        }
    }

    /*
        â ��� �������� FullScreenMode(enum)���� ��ȯ
        int _mode | enum FullScreenMode�� int�� ��
        return | FullScreenMode | default: FullScreenMode.FullScreenWindow
    */
    private FullScreenMode ChangeToFullScreenMode(int _mode)
    {
        if (_mode == (int)UIOptions.EScreenMode.FullScreen ||
            _mode == (int)UIOptions.EScreenMode.Windowed)
        {
            bool isFull = _mode == 0 ? true : false;
            return ChangeToFullScreenMode(isFull);
        }
        else
        {
            switch (_mode)
            {
                case 0:
                    return FullScreenMode.ExclusiveFullScreen;
                case 2:
                    return FullScreenMode.MaximizedWindow;
                case 3:
                    return FullScreenMode.Windowed;
                default:
                    return FullScreenMode.FullScreenWindow;
            }
        }
    }

    /*
        FullScreenMode _mode | ���� enum FullScreenMode�� ��
        return | int | default: 1
    */
    private int ChangeFullScreenModeToOption(FullScreenMode _mode)
    {
        if (_mode == FullScreenMode.ExclusiveFullScreen ||
            _mode == FullScreenMode.MaximizedWindow ||
            _mode == FullScreenMode.FullScreenWindow)
            return 0; // ��üȭ��
        else
            return 1; // â ���
    }

    /*
        �ػ� �������� Resolution ����ü�� ��ȯ
        int _option | ���� ȭ�鿡���� �ػ� int �� | default: 0
        return | Resolution | default: width = 640, height = 360
    */
    private Resolution ChangeToResolution(int _option)
    {
        if (_option < 0 || _option >= (int)UIOptions.EResolution.Count) _option = 0;

        return ChangeToResolution((UIOptions.EResolution)_option);
    }

    /*
        EResolution _resolution | ���� ȭ�鿡���� �ػ� enum �� | _640x360, _960x540, _1280x720, _1600x900, _1920x1080, _2560x1440 | default: _640x360
        return | Resolution | default: width = 640, height = 360
    */
    private Resolution ChangeToResolution(UIOptions.EResolution _resolution)
    {
        Resolution resolution = new Resolution();
        int width = 640;
        int height = 360;

        switch (_resolution)
        {
            case UIOptions.EResolution._640x360:
                width = 640;
                height = 360;
                break;
            case UIOptions.EResolution._960x540:
                width = 960;
                height = 540;
                break;
            case UIOptions.EResolution._1280x720:
                width = 1280;
                height = 720;
                break;
            case UIOptions.EResolution._1600x900:
                width = 1600;
                height = 900;
                break;
            case UIOptions.EResolution._1920x1080:
                width = 1920;
                height = 1080;
                break;
            case UIOptions.EResolution._2560x1440:
                width = 2560;
                height = 1440;
                break;
            //case EScreenRes._3840x2160:
            //    width = 3840;
            //    height = 2160;
            //    break;
            default:
                width = 640;
                height = 360;
                break;
        }

        resolution.width = width;
        resolution.height = height;

        return resolution;
    }

    /*
        ���� Resolution�� Dropdown �ɼ� ������ ��ȯ
        Resolution _resolution | ���� Resolution ��
        return | int | default: 0
    */
    private int ChangeResolutionToOption(Resolution _resolution)
    {
        int width = _resolution.width;
        int height = _resolution.height;

        if (width == 640 && height == 360)
            return 0;
        else if (width == 960 && height == 540)
            return 1;
        else if (width == 1280 && height == 720)
            return 2;
        else if (width == 1600 && height == 900)
            return 3;
        else if (width == 1920 && height == 1080)
            return 4;
        else if (width == 2560 && height == 1440)
            return 5;
        //else if (width == 3840 && height == 2160)
        //    return 6;
        else
            return 0;
    }
    #endregion

}
