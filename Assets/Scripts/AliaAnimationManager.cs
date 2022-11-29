using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AliaAnimEnum
{
    UNKOWN = -1,
    TURN_ON, //Alia turn on anim trigger
}

public class AliaAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator _propFR;
    [SerializeField] private Animator _propFL;
    [SerializeField] private Animator _propRR;
    [SerializeField] private Animator _propRL;
    [SerializeField] private AliaMovement _aliaMovement;

    [SerializeField] private float speed;
    [SerializeField] private float slowSpeed = 1;
    
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float mediumSpeed = 3;
    [SerializeField] private float inputThreshold = 0.1f;

    private Rigidbody _aliaRigidbody;
    
    // Start is called before the first frame update
    void Awake()
    {
        _propFL.SetBool("IsFront", true);
        _propFR.SetBool("IsFront", true);

        _aliaMovement.LandEvent += OnLand;
        _aliaMovement.TakeoffEvent += OnTakeOff;

        _aliaRigidbody = _aliaMovement.gameObject.GetComponent<Rigidbody>();

    }

    void OnLand(object sender, EventArgs e)
    {
        _propFL.SetFloat("Speed", slowSpeed);
        _propFR.SetFloat("Speed", slowSpeed);
        _propRL.SetFloat("Speed", slowSpeed);
        _propRR.SetFloat("Speed", slowSpeed);
    }

    void OnTakeOff(object sender, EventArgs e)
    {
        _propFL.SetFloat("Speed", speed);
        _propFR.SetFloat("Speed", speed);
        _propRL.SetFloat("Speed", speed);
        _propRR.SetFloat("Speed", speed);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!_aliaMovement.IsLanded)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            //float vertical = -_aliaRigidbody.gameObject.transform.rotation.eulerAngles.x;
            float horizontal = Input.GetAxisRaw("Horizontal");
            //float horizontal = _aliaRigidbody.gameObject.transform.rotation.eulerAngles.z;
            float turn = Input.GetAxisRaw("Turn");

            if (vertical > inputThreshold || vertical < -inputThreshold)
            {
                //Pitch
                if(vertical > inputThreshold)
                {
                    //Pitch Forward
                    _propFL.SetFloat("Speed", forwardSpeed);
                    _propFR.SetFloat("Speed", forwardSpeed);
                    _propRL.SetFloat("Speed", speed);
                    _propRR.SetFloat("Speed", speed);
                    
                    if (horizontal > inputThreshold || horizontal < -inputThreshold)
                    {
                        //Pitch and Roll Forward
                        if(horizontal > inputThreshold)
                        {
                            //Pitch and Roll Forward Right
                            PitchAndRollForwardRight();
                        }
                        else if (horizontal < -inputThreshold)
                        {
                            //Pitch and Roll Forward Left
                            PitchAndRollForwardLeft();
                        }
                    }
                
                    if (turn > inputThreshold || turn < -inputThreshold)
                    {
                        //Pitch and Yaw Forward
                        if(turn > inputThreshold)
                        {
                            //Pitch and Yaw Forward Clockwise
                            PitchAndRollForwardRight();

                        }
                        else if (turn < -inputThreshold)
                        {
                            //Pitch and Yaw Forward Counter-Clockwise
                            PitchAndRollForwardLeft();

                        }
                    }
                }
                else if (vertical < -inputThreshold)
                {
                    //Pitch backward
                    _propFL.SetFloat("Speed", speed);
                    _propFR.SetFloat("Speed", speed);
                    _propRL.SetFloat("Speed", forwardSpeed);
                    _propRR.SetFloat("Speed", forwardSpeed);
                    
                    if (horizontal > inputThreshold || horizontal < -inputThreshold)
                    {
                        //Pitch and Roll backward
                        if(horizontal > inputThreshold)
                        {
                            //Pitch and Roll backward Right
                            PitchAndRollBackwardRight();
                        }
                        else if (horizontal < -inputThreshold)
                        {
                            //Pitch and Roll backward Left
                            PitchAndRollBackwardLeft();
                        }
                    }
                    
                    if (turn > inputThreshold || turn < -inputThreshold)
                    {
                        //Pitch and Yaw backward
                        if(turn > inputThreshold)
                        {
                            //Pitch and Yaw backward Clockwise
                            PitchAndRollBackwardLeft();

                            
                        }
                        else if (turn < -inputThreshold)
                        {
                            //Pitch and Yaw backward Counter-Clockwise
                            PitchAndRollBackwardRight();
                        }
                    }
                }
            }
            else if (horizontal > inputThreshold || horizontal < -inputThreshold)
            {
                //Roll
                if(horizontal > inputThreshold)
                {
                    //Roll Right
                    _propFL.SetFloat("Speed", speed);
                    _propFR.SetFloat("Speed", forwardSpeed);
                    _propRL.SetFloat("Speed", speed);
                    _propRR.SetFloat("Speed", forwardSpeed);

                    if (turn > inputThreshold || turn < -inputThreshold)
                    {
                        //Roll and Yaw Right
                        if(turn > inputThreshold)
                        {
                            //Roll and Yaw Right Clockwise
                            PitchAndRollBackwardRight();
                        }
                        else if (turn < -inputThreshold)
                        {
                            //Roll and Yaw Right Counter-Clockwise
                            PitchAndRollForwardRight();
                        }
                    }
                }
                else if (horizontal < -inputThreshold)
                {
                    //Roll Left
                    _propFL.SetFloat("Speed", forwardSpeed);
                    _propFR.SetFloat("Speed", speed);
                    _propRL.SetFloat("Speed", forwardSpeed);
                    _propRR.SetFloat("Speed", speed);
                    
                    if (turn > inputThreshold || turn < -inputThreshold)
                    {
                        //Roll and Yaw Left
                        if(turn > inputThreshold)
                        {
                            //Roll and Yaw Left Clockwise
                            PitchAndRollForwardLeft();
                        }
                        else if (turn < -inputThreshold)
                        {
                            //Roll and Yaw Left Counter-Clockwise
                            PitchAndRollBackwardLeft();
                        }
                    }
                }
            }
            else if (turn > inputThreshold || turn < -inputThreshold)
            {
                //Yaw
                if(turn > inputThreshold)
                {
                    //Yaw Clockwise
                    _propFL.SetFloat("Speed", speed);
                    _propFR.SetFloat("Speed", forwardSpeed);
                    _propRL.SetFloat("Speed", forwardSpeed);
                    _propRR.SetFloat("Speed", speed);
                }
                else if (turn < -inputThreshold)
                {
                    //Yaw Counter-Clockwise
                    _propFL.SetFloat("Speed", forwardSpeed);
                    _propFR.SetFloat("Speed", speed);
                    _propRL.SetFloat("Speed", speed);
                    _propRR.SetFloat("Speed", forwardSpeed);
                }
            }
            else
            {
                //hover
                _propFL.SetFloat("Speed", speed);
                _propFR.SetFloat("Speed", speed);
                _propRL.SetFloat("Speed", speed);
                _propRR.SetFloat("Speed", speed);
            }
        }
    }

    private void PitchAndRollBackwardLeft()
    {
        _propFL.SetFloat("Speed", mediumSpeed);
        _propFR.SetFloat("Speed", speed);
        _propRL.SetFloat("Speed", forwardSpeed);
        _propRR.SetFloat("Speed", mediumSpeed);
    }

    private void PitchAndRollBackwardRight()
    {
        _propFL.SetFloat("Speed", speed);
        _propFR.SetFloat("Speed", mediumSpeed);
        _propRL.SetFloat("Speed", mediumSpeed);
        _propRR.SetFloat("Speed", forwardSpeed);
    }

    private void PitchAndRollForwardLeft()
    {
        _propFL.SetFloat("Speed", forwardSpeed);
        _propFR.SetFloat("Speed", mediumSpeed);
        _propRL.SetFloat("Speed", mediumSpeed);
        _propRR.SetFloat("Speed", speed);
    }

    private void PitchAndRollForwardRight()
    {
        _propFL.SetFloat("Speed", mediumSpeed);
        _propFR.SetFloat("Speed", forwardSpeed);
        _propRL.SetFloat("Speed", speed);
        _propRR.SetFloat("Speed", mediumSpeed);
    }
}
