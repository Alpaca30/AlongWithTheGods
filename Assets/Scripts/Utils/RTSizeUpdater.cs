using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RTSizeUpdater : MonoBehaviour
{
    [SerializeField]
    private RawImage ri = null;

    private Camera cam = null;
    private RenderTexture rt = null;
    
    Resolution rs; // 화면 사이즈 비교용


    private void Awake()
    {
        rs = Screen.currentResolution;
        if(TryGetComponent<Camera>(out cam))
        {
            //int width = Screen.currentResolution.width;
            //int height = Screen.currentResolution.height;
            int width = Screen.width;
            int height = Screen.height;

            rt = cam.targetTexture;
            OnDisplaySizeChanged(width, height);
        }
    }

    private void Update()
    {
        // Screen.width, Screen.height는 테스트에서만 사용.
        // 나중에는 Settings에서 Callback으로 처리하거나 Settings에서 대신 처리할 예정

        //if(rs.width != Screen.currentResolution.width &&
        //    rs.height != Screen.currentResolution.height)
        if(rs.width != Screen.width &&
            rs.height != Screen.height)
        {
            //int width = Screen.currentResolution.width;
            //int height = Screen.currentResolution.height;
            int width = Screen.width;
            int height = Screen.height;

            OnDisplaySizeChanged(width, height);
        }
    }


    public void OnDisplaySizeChanged(int _width, int _height)
    {
        if (rt == null) return;

        // 최소 지원 Size(640, 360)까지만 지원
        if (_width < 640) _width = 640;
        if (_height < 360) _height = 360;
        
        rs.width = _width;
        rs.height = _height;

        if (_width % 2 == 1) _width -= 1;
        if (_height % 2 == 1) _height -= 1;

        SetMatResolution(_width, _height);
        SetRTSize(_width, _height);
    }

    private void SetRTSize(int _width, int _height)
    {
        rt.Release();
        rt.width = _width;
        rt.height = _height;
        rt.Create();
    }

    private void SetMatResolution(int _width, int _height)
    {
        if (ri == null) return;

        Material mat = ri.material;
        mat.SetVector("_Resolution", new Vector2(_width, _height));
#if UNITY_EDITOR
        //Debug.LogFormat("Resolution: {0}", mat.GetVector("_Resolution"));
#endif
    }
}
