using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingEvents : MonoBehaviour
{
    // settings.ini 파일에 설정 값 업데이트 Callback
    public delegate void UpdateSettingsINICallback();
    private UpdateSettingsINICallback updateINICallback = null;

    // 전체 UI상 값을 업데이트하는 Callback
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
        // settings.ini 파일에 설정 값 업데이트 Callback
        updateINICallback = _updateINICallback;

        // 전체 UI상 값을 업데이트하는 Callback
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
    // 시작하기
    public void OnClickStart()
    {
        LoadingSceneManager.LoadScene(LoadingSceneManager.EScene.Base_Scene.ToString());
    }
    // 종료
    public void OnClickQuit()
    {
        Application.Quit();
    }
    #endregion


    #region InGame Setting
    // 이어하기
    public void OnClickContinue()
    {
        if (optionUI == null) return;

        OnCloseMenu();
    }

    // 나가기
    public void OnClickExit()
    {
        // Confirm Prompt 열기
        OnOpenPrompt();
    }
    #endregion


    #region Open/Close MenuUI
    // 메뉴 닫기
    private void OnCloseMenu()
    {
        ActiveMenu(false);
    }
    #endregion


    #region Open/Close OptionUI
    // 설정창 열기
    public void OnOpenOptions()
    {
        // 적용 전 Setting 값 가져오기
        GetCurrentSettings();

        ActiveOption(true);
    }
    // 설정창 닫기
    private void OnCloseOptions()
    {
        ActiveOption(false);
    }
    #endregion


    #region Options Setting
    // 음향 수치 변경
    public void OnChangeMusicVolume(float _vol)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[Music] volume: {0}", _vol);
#endif
        musicVolumeCallback?.Invoke(_vol);
    }
    // SFX 수치 변경
    public void OnChangeSoundEffectsVolume(float _vol)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[SoundEffects] volume: {0}", _vol);
#endif
        soundEffectsVolumeCallback?.Invoke(_vol);
    }
    // 창 모드 변경
    public void OnChangeFullScreenMode(int _option)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[FullScreenMode] option: {0}", _option);
#endif
        fullScreenModeCallback?.Invoke(_option);
    }
    // 해상도 변경
    public void OnChangedResolution(int _option)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[Resolution] option: {0}", _option);
#endif
        resolutionCallback?.Invoke(_option);
    }
    // 품질 변경
    public void OnChangedQuality(int _option)
    {
#if UNITY_EDITOR
        //Debug.LogFormat("[Quality] option: {0}", _option);
#endif
        qualityCallback?.Invoke(_option);
    }

    // 적용
    public void OnClickApplySettings()
    {
        // 적용을 하면 설정한 값으로 변경
        // 이전 값들도 변경
        GetCurrentSettings();
        SetCurrentSettings();
        // 적용하고 나가기
        OnCloseOptions();
    }
    // 취소
    public void OnClickRefuseSettings()
    {
        // 취소를 하면 이전 값으로 되돌림
        SetCurrentSettings();
        OnCloseOptions();
    }
    #endregion


    #region Open/Close PromptUI
    // 확인창 열기
    private void OnOpenPrompt()
    {
        ActivePrompt(true);
    }
    // 확인창 닫기
    private void OnClosePrompt()
    {
        ActivePrompt(false);
    }
    #endregion


    #region PromptUI Setting
    // 진행중 나가기 확인창 - 예
    public void OnClickConfirmPrompt()
    {
        // 이미지를 불러옴
        //LoadingSceneManager.LoadScene(EScene.Main_Scene.ToString());
        ActiveEnding(EndingType.GiveUp, true);
    }
    // 진행중 나가기 확인창 - 아니오
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

    // 재시도 버튼 이벤트
    public void OnClickReTry()
    {
        LoadingSceneManager.LoadScene(LoadingSceneManager.EScene.Play_Scene.ToString());
    }
    // 메인화면으로 돌아가기 버튼
    public void OnExitToMain()
    {
        LoadingSceneManager.LoadScene(LoadingSceneManager.EScene.Main_Scene.ToString());
    }
    #endregion


    #region UI Active
    /*
        MenuUI 활성화/비활성화
        bool _active | 활성화/비활성화
    */
    private void ActiveMenu(bool _active)
    {
        if (menuUI == null) return;

        menuUI.gameObject.SetActive(_active);
    }

    /*
        OptionUI 활성화/비활성화
        bool _active | 활성화/비활성화
    */
    private void ActiveOption(bool _active)
    {
        if (optionUI == null) return;

        optionUI.gameObject.SetActive(_active);
    }

    /*
        PromptUI 활성화/비활성화
        bool _active | 활성화/비활성화
    */
    private void ActivePrompt(bool _active)
    {
        if (promptUI == null) return;

        promptUI.gameObject.SetActive(_active);
    }

    /*
        EndingUI 활성화/비활성화
        bool _active | 활성화/비활성화
    */
    private void ActiveEnding(EndingType _ending, bool _active)
    {
        endingFailed.gameObject.SetActive(false);
        endingGiveUp.gameObject.SetActive(false);
        endingComplete.gameObject.SetActive(false);

        switch (_ending)
        {
            case EndingType.Failed: // 사망 또는 목표 상실시
                endingFailed.gameObject.SetActive(_active);
                return;
            case EndingType.Retry: // 미정..
                return;
            case EndingType.GiveUp: // 게임 도중 나갈 시
                endingGiveUp.gameObject.SetActive(_active);
                return;
            case EndingType.Complete: // 스테잊지 클리어를 할 시
                endingComplete.gameObject.SetActive(_active);
                return;
        }

        //GetEnding(_ending);
    }
    #endregion


    /*
        현재의 Setting값을 가져와서 각 prev변수에 저장함
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
        prev변수에 저장한 값을 현재 Setting값에 적용함
    */
    private void SetCurrentSettings()
    {
        // UI상 값 변경
        updateCallback?.Invoke(
            prevMusicVolume,
            prevSoundEffectsVolume,
            prevFullScreenModeOption,
            prevResolutionOption,
            prevQuality
            );

        // Settings 값 변경
        musicVolumeCallback?.Invoke(prevMusicVolume);
        soundEffectsVolumeCallback?.Invoke(prevSoundEffectsVolume);
        fullScreenModeCallback?.Invoke(prevFullScreenModeOption);
        resolutionCallback?.Invoke(prevResolutionOption);
        qualityCallback?.Invoke(prevQuality);

        // Settings 값 ini파일에 저장
        updateINICallback?.Invoke();
    }

    /*
    각 타입에 맞는 Ending으로 바꿔서 출력함
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
