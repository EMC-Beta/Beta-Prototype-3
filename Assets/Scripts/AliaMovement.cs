using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AliaMovement : MonoBehaviour
{
    Rigidbody rb;

    float speed;
    float tiltAmount;
    Vector3 moveDir;
    Vector3 tiltDir;
    float turn;

    //Horizontal Flight
    [SerializeField] float hoverSpeed = 2f;

    //Pitch and Roll
    float hoverTiltAmount = -10f;
    float tiltSpeed = 5f;

    //Yaw
    [SerializeField] float turnSpeed = 2f;

    //Takeoff Flight
    [SerializeField] float takeoffSpeed = 1f;
    bool landed = true;

    public bool IsLanded
    {
        get
        {
            return landed;
        }
    }
    [SerializeField] float maxHeight = 20;
    [SerializeField] float minHeight = .5f;
    float yPos = 0;
    [SerializeField] GameObject takeoffPanel;

    public event EventHandler LandEvent;
    public event EventHandler TakeoffEvent;

    //Leaving
    [SerializeField] float flightSpeed = 5f;
    [SerializeField] float leavePitchAmount = -20f;
    float pitchAmount = -20f;
    bool leaving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = hoverSpeed;
        tiltAmount = hoverTiltAmount;
    }

    private void Update()
    {
        if(!leaving)
        {
            GetInput();
        }
        else
        {
            Leave();
        }
    }

    void GetInput()
    {
        //Landing Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            landed = !landed;

            if (landed)
            {
                LandEvent?.Invoke(this, new EventArgs());
                rb.velocity = Vector3.zero;
                moveDir = Vector3.zero;
                tiltDir = Vector3.zero;
                turn = 0;
            }
            else
            {
                TakeoffEvent?.Invoke(this, new EventArgs());
                rb.velocity = Vector3.zero;
                moveDir = Vector3.zero;
                tiltDir = Vector3.zero;
                turn = 0;
            }
        }

        //Movement Input
        moveDir = Vector3.zero;
        turn = 0;
        if (!landed)
        {
            tiltDir.x = Input.GetAxisRaw("Vertical");
            tiltDir.z = Input.GetAxisRaw("Horizontal");
            moveDir = (transform.forward * tiltDir.x) + (transform.right * tiltDir.z);

            turn = Input.GetAxisRaw("Turn");
        }
    }

    void Leave()
    {
        tiltDir = -Vector3.right;
        tiltAmount = leavePitchAmount;
        moveDir = transform.forward;
        speed = flightSpeed;
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

        if(!leaving)
        {
            //Slerp to takeoff or landing position
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, yPos, transform.position.z), Time.deltaTime * takeoffSpeed);
        }

        //Lerp velocity based on input
        if(moveDir.sqrMagnitude > 0)
            rb.velocity = Vector3.Lerp(rb.velocity, moveDir.normalized * speed, Time.deltaTime);

        //Lerp tilt (pitch  and roll) rotation based on input-----------------------------------------------Pitch----------Roll--------------------------------Add y rotation back to cancel out change in y
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler((new Vector3(-tiltDir.x, 0, tiltDir.z).normalized * tiltAmount) + (Vector3.up * transform.eulerAngles.y)), Time.deltaTime * tiltSpeed);

        //Lerp yaw based on input
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y +  turn, transform.eulerAngles.z)), Time.deltaTime * turnSpeed);
        transform.Rotate(Vector3.up, turn * turnSpeed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Leave")
        {
            Debug.Log("Leave");
            leaving = true;
            turn = 0;   //Disable turning
        }
    }
}
