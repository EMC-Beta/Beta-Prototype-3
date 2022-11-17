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

    [SerializeField] private float speed;
    [SerializeField] private float forwardSpeed;
    
    // Start is called before the first frame update
    void Awake()
    {
        _propFL.SetBool("IsFront", true);
        _propFR.SetBool("IsFront", true);
        
        _propFL.SetFloat("Speed", speed);
        _propFR.SetFloat("Speed", speed);
        _propRL.SetFloat("Speed", speed);
        _propRR.SetFloat("Speed", speed);
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(vertical > 0.1)
        {
            _propFL.SetFloat("Speed", forwardSpeed);
            _propFR.SetFloat("Speed", forwardSpeed);
            _propRL.SetFloat("Speed", speed);
            _propRR.SetFloat("Speed", speed);
        }
        else if (vertical < -0.1)
        {
            _propFL.SetFloat("Speed", speed);
            _propFR.SetFloat("Speed", speed);
            _propRL.SetFloat("Speed", forwardSpeed);
            _propRR.SetFloat("Speed", forwardSpeed);
        }
        else if(horizontal > 0.1)
        {
            _propFL.SetFloat("Speed", speed);
            _propFR.SetFloat("Speed", forwardSpeed);
            _propRL.SetFloat("Speed", speed);
            _propRR.SetFloat("Speed", forwardSpeed);
        }
        else if (horizontal < -0.1)
        {
            _propFL.SetFloat("Speed", forwardSpeed);
            _propFR.SetFloat("Speed", speed);
            _propRL.SetFloat("Speed", forwardSpeed);
            _propRR.SetFloat("Speed", speed);
        }
        else
        {
            _propFL.SetFloat("Speed", speed);
            _propFR.SetFloat("Speed", speed);
            _propRL.SetFloat("Speed", speed);
            _propRR.SetFloat("Speed", speed);
        }

    }
}
