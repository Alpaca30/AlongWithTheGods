using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;  // 카메라와 대상 오브젝트 간의 거리
    [SerializeField] private Camera renderCamera = null;
    [SerializeField] private Camera uiCamera = null;

    private float timer;
    private Vector3 velocity = Vector3.zero;  // 속도 변수
    public float smoothTime;  // 부드러운 이동을 위한 시간
    public Transform target;  // 카메라가 쫓아갈 대상 오브젝트


    private void Update()
    {
        // 대상 오브젝트를 따라가는 목표 위치 계산
        Vector3 targetPosition = target.position + offset;

        // 현재 위치에서 목표 위치까지 부드러운 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void SetBossRoomCamera()
    {
        StartCoroutine(SetBossRoomCameraCoroutine());
    }

    private IEnumerator SetBossRoomCameraCoroutine()
    {
        while (timer < 5f)
        {
            timer += Time.deltaTime;
            renderCamera.orthographicSize = Mathf.Lerp(7, 11, 0.5f * timer);
            uiCamera.orthographicSize = Mathf.Lerp(7, 11, 0.5f * timer);
            yield return null;
        }
    }
}

