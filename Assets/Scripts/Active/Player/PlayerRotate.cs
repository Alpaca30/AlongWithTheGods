using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    private PlayerMovement pm;
    private float horizontalInput;
    private float lastHorizontalInput = 1;

    private void Awake()
    {
        pm = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        RotatePlayer();
        ClimbRotation();
    }

    private void RotatePlayer()
    {
        if (!pm.climbing && !pm.dead && !pm.hit && !pm.skill && !pm.dashing && !pm.isStoryAction)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            //if (pm.dashing) return;
            
            if (horizontalInput != 0f)
            {
                transform.rotation = Quaternion.Euler(0f, 90f * horizontalInput, 0f);
                lastHorizontalInput = horizontalInput;
            }
            else if (horizontalInput == 0) transform.rotation = Quaternion.Euler(0f, 90f * lastHorizontalInput, 0f);
        }

        if (pm.dead)
        {
            transform.rotation = Quaternion.Euler(0f, 90f * lastHorizontalInput, 0f);
            Debug.Log("asdasd"+lastHorizontalInput);
        }
    }

    private void ClimbRotation()
    {
        if (pm.climbing && !pm.dead)
        {
            if (horizontalInput != 0f)
            {
                transform.rotation = Quaternion.Euler(-90f, 90f * horizontalInput, 0f);
                lastHorizontalInput = horizontalInput;
            }
        }
    }
}
