using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] GameObject[] landTooltips;

    AliaMovement aliaMove;

    private void OnEnable()
    {
        aliaMove = GameObject.Find("ALIA").GetComponent<AliaMovement>();
        aliaMove.LandEvent += Land;
        aliaMove.TakeoffEvent += Takeoff;

        Land(this, new EventArgs());
    }

    private void OnDisable()
    {
        aliaMove.LandEvent -= Land;
        aliaMove.TakeoffEvent -= Takeoff;
    }

    public void Land(object sender, EventArgs e)
    {
        foreach(GameObject obj in landTooltips)
        {
            obj.SetActive(true);
        }
    }

    public void Takeoff(object sender, EventArgs e)
    {
        foreach (GameObject obj in landTooltips)
        {
            obj.SetActive(false);
        }
    }
}
