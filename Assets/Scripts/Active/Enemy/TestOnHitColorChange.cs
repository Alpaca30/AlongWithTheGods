using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOnHitColorChange : MonoBehaviour
{
    private Color originColor;
    private Renderer rd;
    private void Awake()
    {
        rd = GetComponent<Renderer>();
        originColor = rd.material.color;
    }

    public void SetCoroutine()
    {
        StartCoroutine(OnHitColor());
    }

    private IEnumerator OnHitColor()
    {
        rd.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rd.material.color = originColor;
    }
}
