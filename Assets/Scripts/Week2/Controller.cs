using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Camera cam;
    private Rigidbody rb;
    public bool grounded;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;
    public float moveSpeed = 10f;
    public float turnSpeed = 6f;
    public float jumpSpeed = 6f;
    public float maxAirSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //determines whether our bool, grounded, is true or false by seeing if our groundcheck overlaps something on the ground layer
        grounded = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, whatIsGround).Length > 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print(rb.velocity);


        PlayerMovement();
        Jump();

        if (!grounded)
        {
            bool positiveX = rb.velocity.x > 0f;
            bool positiveZ = rb.velocity.z > 0f;
            float capX = Math.Abs(rb.velocity.x) < maxAirSpeed ? rb.velocity.x : positiveX ? maxAirSpeed : -maxAirSpeed;
            float capZ = Math.Abs(rb.velocity.z) < maxAirSpeed ? rb.velocity.z : positiveZ ? maxAirSpeed : -maxAirSpeed;
            
            rb.velocity = new Vector3(capX, -7f, capZ);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //and you are on the ground...
            if(grounded)
            {
                rb.AddForce(new Vector3 (rb.velocity.x, jumpSpeed, rb.velocity.z));
            }
        }
    }
    
    private void PlayerMovement()
    {
        // Get input vals for forward and sideways movement
        float playerForwardMag = Input.GetAxis("Vertical") * Time.deltaTime;
        float playerSideMag = Input.GetAxis("Horizontal") * Time.deltaTime;
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
        rb.velocity = directionToMove * moveSpeed * moveSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
