using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGenerator : MonoBehaviour
{
    public Canvas GenerateCanvas(string _name)
    {
        Canvas canvas = null;

        GameObject go = new GameObject(_name, typeof(Canvas), typeof(CanvasScaler));
        if (go.TryGetComponent<Canvas>(out canvas))
        {
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            scaler.referenceResolution = new Vector2(960f, 540f);

            return canvas;
        }

        return null;
    }
}
