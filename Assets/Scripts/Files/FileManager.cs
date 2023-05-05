using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class FileManager
{
    // Dialogue
    public string GetScriptData(string _did)
    {
        string dialogue = "/Dialogues/dialogue_";
        string dialoguePath = Application.streamingAssetsPath + dialogue;
        StringBuilder sbPath = new StringBuilder();
        sbPath.Append(dialoguePath);
        sbPath.Append(_did);
        string path = sbPath.ToString();
#if UNITY_EDITOR
        //Debug.LogFormat("path: {0}", path);
#endif

        if (File.Exists(path)) // 파일이 존재한다면 파일 읽기
        {
#if UNITY_EDITOR
            //Debug.Log("Exist");
#endif
            using (StreamReader sr = File.OpenText(path))
            {
                string fileData = sr.ReadToEnd();
                Regex regex = new Regex(@"\s+");
                fileData = regex.Replace(fileData, " ");
#if UNITY_EDITOR
                //Debug.LogFormat("[FileManager] {0}", fileData);
#endif
                return fileData;
            }
        }
#if UNITY_EDITOR
        //Debug.Log("Return Empty!");
#endif
        return string.Empty;
    }

    public Dictionary<string, Dictionary<string, string>> ReadIniData()
    {
        // 읽어들인 Data를 담을 Dictionary
        // [section]
        // key = value
        Dictionary<string, Dictionary<string, string>> sections = null;

        string path = Path.Combine(Application.persistentDataPath, "settings.ini");
        if (File.Exists(path))
        {
            sections = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> audio = new Dictionary<string, string>();
            Dictionary<string, string> graphics = new Dictionary<string, string>();

            string musicVolume = INIWorker.IniReadValue(INIWorker.Sections.audio, INIWorker.Keys.musicVolume);
            string soundEffectsVolume = INIWorker.IniReadValue(INIWorker.Sections.audio, INIWorker.Keys.soundEffectsVolume);

            string fullscreen = INIWorker.IniReadValue(INIWorker.Sections.graphics, INIWorker.Keys.fullscreen);
            string width = INIWorker.IniReadValue(INIWorker.Sections.graphics, INIWorker.Keys.width);
            string height = INIWorker.IniReadValue(INIWorker.Sections.graphics, INIWorker.Keys.height);
            string quality = INIWorker.IniReadValue(INIWorker.Sections.graphics, INIWorker.Keys.quality);

            audio.Add("musicVolume", musicVolume);
            audio.Add("soundEffectsVolume", soundEffectsVolume);
            sections.Add("audio", audio);

            graphics.Add("fullscreen", fullscreen);
            graphics.Add("width", width);
            graphics.Add("height", height);
            graphics.Add("quality", quality);
            sections.Add("graphics", graphics);
        }

        return sections;
    }

    public void WriteIniData(float _musicVolume, float _soundEffectsVolume, int _fullScreenMode, int _width, int _height, int _quality)
    {
        // 다른 섹션에 설정값이 들어가는 버그가 있음. 차후 수정 필요. (INIWorker)
        // 예) [graphics] 섹션에 'soundEffectsVolume = 1'가 들어감
        INIWorker.IniWriteValue(INIWorker.Sections.audio, INIWorker.Keys.musicVolume, _musicVolume.ToString());
        INIWorker.IniWriteValue(INIWorker.Sections.audio, INIWorker.Keys.soundEffectsVolume, _soundEffectsVolume.ToString());
        INIWorker.IniWriteValue(INIWorker.Sections.graphics, INIWorker.Keys.fullscreen, _fullScreenMode.ToString());
        INIWorker.IniWriteValue(INIWorker.Sections.graphics, INIWorker.Keys.width, _width.ToString());
        INIWorker.IniWriteValue(INIWorker.Sections.graphics, INIWorker.Keys.height, _height.ToString());
        INIWorker.IniWriteValue(INIWorker.Sections.graphics, INIWorker.Keys.quality, _quality.ToString());
    }
}
