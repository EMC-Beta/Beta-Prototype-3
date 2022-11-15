using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AliaAnimEnum
{
    UNKOWN = -1,
    TURN_ON, //Alia turn on anim trigger
}

[RequireComponent(typeof(Animator))]
public class AliaAnimationManager : MonoBehaviour
{
    private Animator AliaAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        AliaAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AliaAnim.SetTrigger((int)AliaAnimEnum.TURN_ON);
        }
    }
}
