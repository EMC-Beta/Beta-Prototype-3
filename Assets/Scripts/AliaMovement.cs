using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AliaMovement : MonoBehaviour
{
    Rigidbody rb;

    Vector3 moveDir;
    Vector3 tiltDir;
    float turn;

    //Horizontal Flight
    [SerializeField] float speed = 1f;

    //Pitch and Roll
    float tiltAmount = -10f;
    float tiltSpeed = 5f;

    //Yaw
    [SerializeField] float turnSpeed = 2f;

    //Takeoff Flight
    [SerializeField] float takeoffSpeed = 1f;
    bool landed = true;
    float maxHeight = 2;
    float minHeight = .5f;
    float yPos = 0;

    public event EventHandler LandEvent;
    public event EventHandler TakeoffEvent;

    [SerializeField] GameObject takeoffPanel;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Landing Input
        if(Input.GetKeyDown(KeyCode.Space))
        {
            landed = !landed;

            if (landed)
            {
                LandEvent?.Invoke(this, new EventArgs());
            }
            else
            {
                TakeoffEvent?.Invoke(this, new EventArgs());
            }
        }

        //Movement Input
        moveDir = Vector3.zero;
        turn = 0;
        if (!landed)
        {
            //moveDir.x = Input.GetAxisRaw("Horizontal");
            //moveDir.z = Input.GetAxisRaw("Vertical");
            tiltDir.x = Input.GetAxisRaw("Vertical");
            tiltDir.z = Input.GetAxisRaw("Horizontal");
            moveDir = (transform.forward * tiltDir.x) + (transform.right * tiltDir.z);

            turn = Input.GetAxisRaw("Turn");
        }
    }

    private void FixedUpdate()
    {
        //Set target y position for takeoff and landing
        if(landed)
        {
            yPos = minHeight;
        }
        else
        {
            yPos = maxHeight;
        }

        //Slerp to takeoff or landing position
        transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, yPos, transform.position.z), Time.deltaTime * takeoffSpeed);

        //Lerp velocity based on input
        rb.velocity = Vector3.Lerp(rb.velocity, moveDir.normalized * speed, Time.deltaTime);

        //Lerp tilt (pitch  and roll) rotation based on input-----------------------------------------------Pitch----------Roll--------------------------------Add y rotation back to cancel out change in y
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler((new Vector3(-tiltDir.x, 0, tiltDir.z).normalized * tiltAmount) + (Vector3.up * transform.eulerAngles.y)), Time.deltaTime * tiltSpeed);

        //Lerp yaw based on input
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y +  turn, transform.eulerAngles.z)), Time.deltaTime * turnSpeed);
        transform.Rotate(Vector3.up, turn * turnSpeed, Space.World);
    }
}
