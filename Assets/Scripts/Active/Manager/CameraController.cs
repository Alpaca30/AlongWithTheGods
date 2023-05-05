using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;  // ī�޶�� ��� ������Ʈ ���� �Ÿ�
    [SerializeField] private Camera renderCamera = null;
    [SerializeField] private Camera uiCamera = null;

    private float timer;
    private Vector3 velocity = Vector3.zero;  // �ӵ� ����
    public float smoothTime;  // �ε巯�� �̵��� ���� �ð�
    public Transform target;  // ī�޶� �Ѿư� ��� ������Ʈ


    private void Update()
    {
        // ��� ������Ʈ�� ���󰡴� ��ǥ ��ġ ���
        Vector3 targetPosition = target.position + offset;

        // ���� ��ġ���� ��ǥ ��ġ���� �ε巯�� �̵�
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

