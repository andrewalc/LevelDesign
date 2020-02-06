using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class MirrorControl : MonoBehaviour
{
    public Transform reflectionPoint;
    public int verticalAngleIncrements;
    public int horizontalAngleIncrements;
    private int[] _verticalAngles; // The angles that vertical cycling activation will apply
    public int verticalAnglesIndex;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private bool _inPlayerControl;
    private void Start()
    {
        _inPlayerControl = false;
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _verticalAngles = new[] {-verticalAngleIncrements, -verticalAngleIncrements, verticalAngleIncrements * 2}; // +x, +2x, back to 0
    }

    // Update is called once per frame
    void Update()
    {
        if (_inPlayerControl)
        {
            // Left click, horizontal clockwise
            if (Input.GetMouseButtonDown(0))
            {
                transform.Rotate(Vector3.up, horizontalAngleIncrements, Space.World);
            }
            // Right click, horizontal counter-clockwise
            if (Input.GetMouseButtonDown(1))
            {
                transform.Rotate(Vector3.up, -horizontalAngleIncrements, Space.World);
            }
            // E for verticle angles
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.Rotate(Vector3.right, _verticalAngles[verticalAnglesIndex], Space.Self);
                verticalAnglesIndex = (verticalAnglesIndex + 1) % _verticalAngles.Length;
            }
            // Q to reset to original state
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.position = _originalPosition;
                transform.rotation = _originalRotation;
            }
        }
    }

    public void SetControl(bool isActive)
    {
        _inPlayerControl = isActive;
    }

    public bool IsVerticleNeutral()
    {
        return verticalAnglesIndex == 0;
    }
}
