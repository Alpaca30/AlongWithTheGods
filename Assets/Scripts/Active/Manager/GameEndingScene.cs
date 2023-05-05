using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndingScene : MonoBehaviour
{
    [SerializeField] private Image image = null;
    [SerializeField] private RectTransform endingImage = null;

    Color c;
    private void Awake()
    {
        c = image.color;
        c = Color.black;
        c.a = 0f;
        image.color = c;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            StartFade();
            _other.GetComponent<PlayerMovement>().ChangeStoryAction();
        }
    }

    public void StartFade()
    {
        StartCoroutine(FadeInOutStart());
    }

    public IEnumerator FadeInOutStart()
    {
        for (float f = 0f; f < 1; f += 0.01f)
        {
            c.a = f;
            image.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        endingImage.gameObject.SetActive(true);
    }
}
