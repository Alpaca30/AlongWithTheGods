using UnityEngine;
using TMPro;

public class UIOptions
{
    public enum EScreenMode
    {
        FullScreen, Windowed
    };
    public enum EResolution
    {
        _640x360, _960x540, _1280x720, _1600x900, _1920x1080, _2560x1440/*, _3840x2160*/, Count
    };
    public enum EQuality
    {
        VeryLow, Low, Medium, High, VeryHigh, Ultra, Count
    };

    [SerializeField]
    private TMP_Dropdown resolDropdown = null;
    [SerializeField]
    private TMP_Dropdown qualityDropdown = null;

    //private List<string> listRes = new List<string>();
    //private List<string> listQual = new List<string>();


    private void Awake()
    {
        if (resolDropdown != null)
        {
            //SetResolutionList();
        }
        if (qualityDropdown != null)
        {
            //SetQualityList();
        }
    }


    //private void SetResolutionList()
    //{
    //    EResolution res;
    //    for (int i = 0; i < (int)EResolution.Count; ++i)
    //    {
    //        res = (EResolution)i;
    //        string option = res.ToString();
    //        option = option.Trim('_').Replace("x", " x ");
    //        listRes.Add(option);
    //    }
    //    resolDropdown.AddOptions(listRes);
    //}

    //private void SetQualityList()
    //{
    //    EQuality qual;
    //    for (int i = 0; i < (int)EQuality.Count; ++i)
    //    {
    //        qual = (EQuality)i;
    //        string option = qual.ToString();
    //        option = Regex.Replace(option, @"_", " ");
    //        listQual.Add(option);
    //    }
    //}
}
