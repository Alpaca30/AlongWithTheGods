using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingEvents : MonoBehaviour
{
    // settings.ini ���Ͽ� ���� �� ������Ʈ Callback
    public delegate void UpdateSettingsINICallback();
    private UpdateSettingsINICallback updateINICallback = null;

    // ��ü UI�� ���� ������Ʈ�ϴ� Callback
    public delegate void UpdateSettingUICallback(
        float _musicVolume,
        float _soundEffectsVolume,
        int _fullScreenMode,
        int _resolution,
        int _quality
        );
    private UpdateSettingUICallback updateCallback = null;

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

    // OnChange Events Callback
    public delegate void OnChangedMusicVolumeCallback(float _vol);
    public delegate void OnChangedSoundEffectsVolumeCallback(float _vol);
    public delegate void OnChangedFullScreenModeCallback(int _option);
    public delegate void OnChangedResolutionCallback(int _option);
    public delegate void OnChangedQualityCallback(int _lv);
    private OnChangedMusicVolumeCallback musicVolumeCallback = null;
    private OnChangedSoundEffectsVolumeCallback soundEffectsVolumeCallback = null;
    private OnChangedFullScreenModeCallback fullScreenModeCallback = null;
    private OnChangedResolutionCallback resolutionCallback = null;
    private OnChangedQualityCallback qualityCallback = null;

    private enum EndingType { Failed, Retry, GiveUp, Complete }

    [SerializeField]
    private RectTransform menuUI = null; // Object or Prefab
    [SerializeField]
    private RectTransform optionUI = null; // Object or Prefab
    [SerializeField]
    private RectTransform promptUI = null; // Object or Prefab
    [Space]
    [SerializeField]
    private RectTransform endingFailed = null;
    [SerializeField]
    private RectTransform endingGiveUp = null;
    [SerializeField]
    private RectTransform endingComplete = null;

    private float prevMusicVolume;
    private float prevSoundEffectsVolume;
    private int prevFullScreenModeOption;
    private int prevResolutionOption;
    private int prevQuality = 0;


    public void Init(
        UpdateSettingsINICallback _updateINICallback,

        UpdateSettingUICallback _updateCallback,

        GetMusicVolumeCallback _getMusicVolume,
        GetSoundEffectsVolumeCallback _getSoundEffectsVolume,
        GetFullScreenModeOptionCallback _getFullScreenModeOption,
        GetResolutionOptionCallback _getResolutionOption,
        GetQualityCallback _getQuality,

        OnChangedMusicVolumeCallback _musicVolumeCallback,
        OnChangedSoundEffectsVolumeCallback _soundEffectsVolumeCallback,
        OnChangedFullScreenModeCallback _fullScreenModeCallback,
        OnChangedResolutionCallback _resolutionCallback,
        OnChangedQualityCallback _qualityCallback
        )
    {
        // settings.ini ���Ͽ� ���� �� ������Ʈ Callback
        updateINICallback = _updateINICallback;

        // ��ü UI�� ���� ������Ʈ�ϴ� Callback
        updateCallback = _updateCallback;

        // Get Settings Value Callback
        getMusicVolume = _getMusicVolume;
        getSoundEffectsVolume = _getSoundEffectsVolume;
        getFullScreenModeOption = _getFullScreenModeOption;
        getResolutionOption = _getResolutionOption;
        getQuality = _getQuality;

        // OnChange Events Callback
        musicVolumeCallback = _musicVolumeCallback;
        soundEffectsVolumeCallback = _soundEffectsVolumeCallback;
        fullScreenModeCallback = _fullScreenModeCallback;
        resolutionCallback = _resolutionCallback;
        qualityCallback = _qualityCallback;
    }


    #region Main Setting
    // �����ϱ�
    public void OnClickStart()
    {
        LoadingSceneManager.LoadScene(LoadingSceneManager.EScene.Base_Scene.ToString());
    }
    // ����
    public void OnClickQuit()
    {
        Application.Quit();
    }
    #endregion


    #region InGame Setting
    // �̾��ϱ�
    public void OnClickContinue()
    {
        if (optionUI == null) return;

        OnCloseMenu();
    }

    // ������
    public void OnClickExit()
    {
        // Confirm Prompt ����
        OnOpenPrompt();
    }
    #endregion


    #region Open/Close MenuUI
    // �޴� �ݱ�
    private void OnCloseMenu()
    {
        ActiveMenu(false);
    }
    #endregion


    #region Open/Close OptionUI
    // ����â ����
    public void OnOpenOptions()
    {
        // ���� �� Setting �� ��������
        GetCurrentSettings();

        ActiveOption(true);
    }
    // ����â �ݱ�
    private void OnCloseOptions()
    {
        ActiveOption(false);
    }
    #endregion


    #region Options Setting
    // ���� ��ġ ����
    public void OnChangeMusicVolume(float _vol)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[Music] volume: {0}", _vol);
#endif
        musicVolumeCallback?.Invoke(_vol);
    }
    // SFX ��ġ ����
    public void OnChangeSoundEffectsVolume(float _vol)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[SoundEffects] volume: {0}", _vol);
#endif
        soundEffectsVolumeCallback?.Invoke(_vol);
    }
    // â ��� ����
    public void OnChangeFullScreenMode(int _option)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[FullScreenMode] option: {0}", _option);
#endif
        fullScreenModeCallback?.Invoke(_option);
    }
    // �ػ� ����
    public void OnChangedResolution(int _option)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[Resolution] option: {0}", _option);
#endif
        resolutionCallback?.Invoke(_option);
    }
    // ǰ�� ����
    public void OnChangedQuality(int _option)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[Quality] option: {0}", _option);
#endif
        qualityCallback?.Invoke(_option);
    }

    // ����
    public void OnClickApplySettings()
    {
        // ������ �ϸ� ������ ������ ����
        // ���� ���鵵 ����
        GetCurrentSettings();
        SetCurrentSettings();
        // �����ϰ� ������
        OnCloseOptions();
    }
    // ���
    public void OnClickRefuseSettings()
    {
        // ��Ҹ� �ϸ� ���� ������ �ǵ���
        SetCurrentSettings();
        OnCloseOptions();
    }
    #endregion


    #region Open/Close PromptUI
    // Ȯ��â ����
    private void OnOpenPrompt()
    {
        ActivePrompt(true);
    }
    // Ȯ��â �ݱ�
    private void OnClosePrompt()
    {
        ActivePrompt(false);
    }
    #endregion


    #region PromptUI Setting
    // ������ ������ Ȯ��â - ��
    public void OnClickConfirmPrompt()
    {
        // �̹����� �ҷ���
        //LoadingSceneManager.LoadScene(EScene.Main_Scene.ToString());
        ActiveEnding(EndingType.GiveUp, true);
    }
    // ������ ������ Ȯ��â - �ƴϿ�
    public void OnClickDenyPrompt()
    {
        OnClosePrompt();
    }
    #endregion


    #region Ending UI Setting
    public void OnDisplayFailed()
    {
        ActiveEnding(EndingType.Failed, true);
    }
    public void OnDisplayRetry()
    {
        ActiveEnding(EndingType.Retry, true);
    }
    public void OnDisplayGiveUp()
    {
        ActiveEnding(EndingType.GiveUp, true);
    }
    public void OnDisplayComplete()
    {
        ActiveEnding(EndingType.Complete, true);
    }

    // ��õ� ��ư �̺�Ʈ
    public void OnClickReTry()
    {
        LoadingSceneManager.LoadScene(LoadingSceneManager.EScene.Play_Scene.ToString());
    }
    // ����ȭ������ ���ư��� ��ư
    public void OnExitToMain()
    {
        LoadingSceneManager.LoadScene(LoadingSceneManager.EScene.Main_Scene.ToString());
    }
    #endregion


    #region UI Active
    /*
        MenuUI Ȱ��ȭ/��Ȱ��ȭ
        bool _active | Ȱ��ȭ/��Ȱ��ȭ
    */
    private void ActiveMenu(bool _active)
    {
        if (menuUI == null) return;

        menuUI.gameObject.SetActive(_active);
    }

    /*
        OptionUI Ȱ��ȭ/��Ȱ��ȭ
        bool _active | Ȱ��ȭ/��Ȱ��ȭ
    */
    private void ActiveOption(bool _active)
    {
        if (optionUI == null) return;

        optionUI.gameObject.SetActive(_active);
    }

    /*
        PromptUI Ȱ��ȭ/��Ȱ��ȭ
        bool _active | Ȱ��ȭ/��Ȱ��ȭ
    */
    private void ActivePrompt(bool _active)
    {
        if (promptUI == null) return;

        promptUI.gameObject.SetActive(_active);
    }

    /*
        EndingUI Ȱ��ȭ/��Ȱ��ȭ
        bool _active | Ȱ��ȭ/��Ȱ��ȭ
    */
    private void ActiveEnding(EndingType _ending, bool _active)
    {
        endingFailed.gameObject.SetActive(false);
        endingGiveUp.gameObject.SetActive(false);
        endingComplete.gameObject.SetActive(false);

        switch (_ending)
        {
            case EndingType.Failed: // ��� �Ǵ� ��ǥ ��ǽ�
                endingFailed.gameObject.SetActive(_active);
                return;
            case EndingType.Retry: // ����..
                return;
            case EndingType.GiveUp: // ���� ���� ���� ��
                endingGiveUp.gameObject.SetActive(_active);
                return;
            case EndingType.Complete: // �������� Ŭ��� �� ��
                endingComplete.gameObject.SetActive(_active);
                return;
        }

        //GetEnding(_ending);
    }
    #endregion


    /*
        ������ Setting���� �����ͼ� �� prev������ ������
    */
    private void GetCurrentSettings()
    {
        prevMusicVolume = (float)getMusicVolume?.Invoke();
        prevSoundEffectsVolume = (float)getSoundEffectsVolume?.Invoke();
        prevFullScreenModeOption = (int)getFullScreenModeOption?.Invoke();
        prevResolutionOption = (int)getResolutionOption?.Invoke();
        prevQuality = (int)getQuality?.Invoke();
    }

    /*
        prev������ ������ ���� ���� Setting���� ������
    */
    private void SetCurrentSettings()
    {
        // UI�� �� ����
        updateCallback?.Invoke(
            prevMusicVolume,
            prevSoundEffectsVolume,
            prevFullScreenModeOption,
            prevResolutionOption,
            prevQuality
            );

        // Settings �� ����
        musicVolumeCallback?.Invoke(prevMusicVolume);
        soundEffectsVolumeCallback?.Invoke(prevSoundEffectsVolume);
        fullScreenModeCallback?.Invoke(prevFullScreenModeOption);
        resolutionCallback?.Invoke(prevResolutionOption);
        qualityCallback?.Invoke(prevQuality);

        // Settings �� ini���Ͽ� ����
        updateINICallback?.Invoke();
    }

    /*
    �� Ÿ�Կ� �´� Ending���� �ٲ㼭 �����
    */
    //private void GetEnding(EndingType _ending)
    //{
    //    string endingName = Enum.GetName(typeof(EndingType), _ending);
    //    string tail = "_Ending";
    //    RectTransform[] animators = endingUI.GetComponentsInChildren<RectTransform>();
    //    RectTransform animator = Array.Find(animators, dummy => dummy.name.Split(tail)[0] == endingName);
    //
    //    for (int i = 0; i < animators.Length; ++i)
    //        animators[i].gameObject.SetActive(false);
    //
    //    if (animator != null)
    //        animator.gameObject.SetActive(true);
    //}
}
