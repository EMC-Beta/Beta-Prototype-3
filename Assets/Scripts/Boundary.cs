using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    //Direction ALIA should be moved back in when colliding with a barrier
    [SerializeField] Vector3 reflectDirection = Vector3.zero;

    public Vector3 BoundaryDirection
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

    //Seconds before barrier pushes player back
    //This way barrier is soft and player can't collide with it like it is solid when the player has low velocity
    //Each boundary needs their own timer value because otherwise, ALIA can escape the area by resetting the timer by flying between a corner barrier
    //one of which will keep pushing them out and resetting the timer
    [SerializeField] float maxBarrierGracePeriod = 3f;
    public float MaxBarrierGracePeriod
    {
        get { return maxBarrierGracePeriod; }
    }

    float barrierGracePeriod;

    //Don't want the variable visible in inspector but still want to change it in code
    public float GetBarrierGracePeriod() { return barrierGracePeriod; }
    public void SetBarrierGracePeriod(float val) { barrierGracePeriod = val; }

    private void Start()
    {
        barrierGracePeriod = maxBarrierGracePeriod;

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
