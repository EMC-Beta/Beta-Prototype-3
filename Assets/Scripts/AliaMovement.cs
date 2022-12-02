using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

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

    //Controls whether ALIA takes off as soon as the game loads or if it starts landed and the player has to press space first
    [SerializeField] bool landed = true;

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
    //bool leaving = false;

    bool barrierCollide = false;

    [SerializeField] private PlayableDirector transitTimeline;
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private GameObject wallsGameObject;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = hoverSpeed;
        tiltAmount = hoverTiltAmount;
    }

    private void Update()
    {
        if(!barrierCollide)
        {
            GetInput();
        }
    }

    void GetInput()
    {
        //Landing Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if (landed)
            //{
            //    TakeOff();
            //}
            //else
            //{
            //    Land();
            //}
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TransitionToTransit();
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

    public void Land()
    {
        landed = true;
        TakeoffEvent?.Invoke(this, new EventArgs());
        rb.velocity = Vector3.zero;
        moveDir = Vector3.zero;
        tiltDir = Vector3.zero;
        turn = 0;
    }

    public void TakeOff()
    {
        landed = false;
        LandEvent?.Invoke(this, new EventArgs());
        rb.velocity = Vector3.zero;
        moveDir = Vector3.zero;
        tiltDir = Vector3.zero;
        turn = 0;
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

        //Lerp to takeoff or landing position
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, yPos, transform.position.z), Time.deltaTime * takeoffSpeed);

        //Lerp velocity based on input
        if(moveDir.sqrMagnitude > 0)
            rb.velocity = Vector3.Lerp(rb.velocity, moveDir.normalized * speed, Time.deltaTime);

        if(!barrierCollide)
        {
            //Lerp tilt (pitch  and roll) rotation based on input-----------------------------------------------Pitch----------Roll--------------------------------Add y rotation back to cancel out change in y
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler((new Vector3(-tiltDir.x, 0, tiltDir.z).normalized * tiltAmount) + (Vector3.up * transform.eulerAngles.y)), Time.deltaTime * tiltSpeed);
        }
        //If tilting away from barrier, use world space rotation so it tilts back (away) when moving forward into barrier or right (also away) when moving left into barrier for example
        else
        {
            //Lerp tilt (pitch  and roll) rotation based on barrier's move direction
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler((new Vector3(-tiltDir.x, 0, tiltDir.z).normalized * tiltAmount) + (Vector3.up * transform.eulerAngles.y)), Time.deltaTime * tiltSpeed);
        }
        

        //Lerp yaw based on input
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y +  turn, transform.eulerAngles.z)), Time.deltaTime * turnSpeed);
        transform.Rotate(Vector3.up, turn * turnSpeed, Space.World);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Boundary>(out Boundary otherBoundary))
        {
            //We want to wait a little before pushing the player back
            //Otherwise, if they have low velocity, the trigger acts like a hard collider
            float time = otherBoundary.GetBarrierGracePeriod();
            if (time <= 0)
            {
                turn = 0;
                moveDir = otherBoundary.BoundaryDirection;
                tiltDir = (transform.forward * moveDir.x) + (transform.right * moveDir.z);
                //tiltDir = new Vector3(moveDir.z, 0, moveDir.x);
                barrierCollide = true;
            }
            else
            {
                otherBoundary.SetBarrierGracePeriod(time - Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Boundary>(out Boundary otherBoundary))
        {
            barrierCollide = false;
            //Reset timer
            otherBoundary.SetBarrierGracePeriod(otherBoundary.MaxBarrierGracePeriod);
        }
    }

    public void TransitionToTransit()
    {
        pathFollower.enabled = true;
        pathFollower.transform.parent.transform.parent = null;
        pathFollower.pathCreator.transform.parent = null;
        transitTimeline.Play();
        wallsGameObject.SetActive(false);
    }
}
