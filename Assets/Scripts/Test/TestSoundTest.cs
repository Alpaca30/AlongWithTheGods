using System;
using UnityEngine;
using UnityEngine.Audio;

public class TestSoundTest : MonoBehaviour
{
    public static TestSoundTest Instance;

    [SerializeField]
    private AudioMixerGroup musicMixerGroup;
    [SerializeField]
    private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField]
    private Sound[] sounds;


    private void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            switch (s.audioType)
            {
                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;

                case Sound.AudioTypes.soundEffect:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;
            }

            if (s.playOnAwake && s.audioType == Sound.AudioTypes.music)
                s.source.Play();
            else if (s.playOnAwake && s.audioType == Sound.AudioTypes.soundEffect)
                s.source.PlayOneShot(s.audioClip, s.volume);
        }
    }


    public void Play(string _clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == _clipname);
        if (s == null)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("Sound: {0} does Not exist!", _clipname);
#endif
            return;
        }
        if (s.audioType == Sound.AudioTypes.music)
            s.source.Play();
        else if (s.audioType == Sound.AudioTypes.soundEffect)
            s.source.PlayOneShot(s.audioClip, s.volume);
    }

    public void Stop(string _clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == _clipname);
        if (s == null)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("Sound: {0} does Not exist!", _clipname);
#endif
            return;
        }
        s.source.Stop();
    }

    public float GetMixerVolume(string _name)
    {
        if (_name == null || _name == string.Empty || _name == "") _name = "";

        float vol;
        if (musicMixerGroup.audioMixer.GetFloat(_name + " Volume", out vol) == false)
            vol = Mathf.Log10(1f) * 20;

        return vol;
    }

    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(SoundOptionManager.musicVolume) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("Sound Effects Volume", Mathf.Log10(SoundOptionManager.soundEffectsVolume) * 20);
    }
}