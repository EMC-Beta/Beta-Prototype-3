using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    //Direction ALIA should be moved back in when colliding with a barrier
    [SerializeField] Vector3 reflectDirection = Vector3.zero;

    public Vector3 boundaryDirection
    {
        get { return reflectDirection; }
    }

    enum Directions
    {
        CUSTOM = 0,
        FORWARD,
        BACK,
        LEFT,
        RIGHT,
        UP,
        DOWN
    };

    //Overrides reflectDirection (unless set to CUSTOM) so you don't have to manually determine it unless you need to
    //This is in local space, transform.<direction> not Vector3.<direction>
    [SerializeField] Directions direction = Directions.CUSTOM;

    private void Start()
    {
        switch(direction)
        {
            case Directions.CUSTOM:
                break;
            case Directions.FORWARD:
                reflectDirection = transform.forward;
                break;
            case Directions.BACK:
                reflectDirection = -transform.forward;
                break;
            case Directions.LEFT:
                reflectDirection = -transform.right;
                break;
            case Directions.RIGHT:
                reflectDirection = transform.right;
                break;
            case Directions.UP:
                reflectDirection = transform.up;
                break;
            case Directions.DOWN:
                reflectDirection = -transform.up;
                break;
        }
    }
}
