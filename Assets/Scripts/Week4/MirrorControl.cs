using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class MirrorControl : MonoBehaviour
{
    public int verticalAngleIncrements;
    public int horizontalAngleIncrements;
    private int[] verticalAngles; // The angles that vertical cycling activation will apply
    private int verticalAnglesIndex;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool inPlayerControl;
    private void Start()
    {
        inPlayerControl = false;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        verticalAnglesIndex = 0;
        verticalAngles = new[] {-verticalAngleIncrements, -verticalAngleIncrements, verticalAngleIncrements * 3,
            verticalAngleIncrements, -verticalAngleIncrements * 2}; // +x, +2x, -x , -2x, back to 0
    }

    // Update is called once per frame
    void Update()
    {
        if (inPlayerControl)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.Rotate(Vector3.up, horizontalAngleIncrements, Space.World);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.Rotate(Vector3.right, verticalAngles[verticalAnglesIndex], Space.Self);
                verticalAnglesIndex = (verticalAnglesIndex + 1) % verticalAngles.Length;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }
        }
    }

    public void SetControl(bool isActive)
    {
        inPlayerControl = isActive;
    }
}
