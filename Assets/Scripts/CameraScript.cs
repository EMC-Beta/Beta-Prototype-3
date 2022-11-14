using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform aliaTrans;

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, aliaTrans.eulerAngles.y, 0);
    }
}
