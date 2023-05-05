using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBox : MonoBehaviour
{
    public Vector3 targetPosition;  // 이동할 목표 위치
    private Vector3 startPos;
    public float speed = 5f;  // 이동 속도
    public bool isOpen = true;

    private void Start()
    {
        startPos = transform.position;
        targetPosition = startPos;
    }

    private void Update()
    {
        MoveDoor();
    }

    private void MoveDoor()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void SetTargetPosition()
    {
        if (isOpen)
        {
            targetPosition = transform.position + new Vector3(0f, 10f, 0f);
            isOpen = false;
            SoundManager.Instance.PlayOneShot("DoorClose");
            Debug.Log("123");
        }
    }

    public void SetStartPos()
    {
        targetPosition = startPos;
        SoundManager.Instance.PlayOneShot("DoorOpen");
        Debug.Log("456");
    }
}
