using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Camera cam;
    private Rigidbody rb;
    public float moveSpeed = 10f;
    public float turnSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
    
    private void PlayerMovement()
    {
        // Get input vals for forward and sideways movement
        float playerForwardMag = Input.GetAxis("Vertical") * Time.deltaTime;
        float playerSideMag = Input.GetAxis("Horizontal") * Time.deltaTime;
        print(playerForwardMag);
        // Player model rotates to face cameras rotation through lerp
        Quaternion camRotation = cam.transform.rotation;
        camRotation.x = 0;
        camRotation.z = 0;

        // Get the camera's vectors for forward and sideways, cancel y, and normalize
        Vector3 camForward = cam.transform.forward;
        Vector3 camSide = cam.transform.right;
        camForward.y = 0;
        camSide.y = 0;
        camSide = camSide.normalized;
        camForward = camForward.normalized;
        // Get the direction the player is moving in relative to the camera
        Vector3 directionToMove = (camForward * playerForwardMag + camSide * playerSideMag);

        //  If the player in moving, rotate the player to the direction the stick is going in 
        if (Mathf.Abs(playerForwardMag) > 0f || Mathf.Abs(playerSideMag) > 0f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionToMove),
                Time.deltaTime * turnSpeed);
        }

        // Multiple camera's direction by input magnitude to get new relative position
        print(directionToMove * moveSpeed * moveSpeed);
        rb.velocity = directionToMove * moveSpeed * moveSpeed;
    }
}
