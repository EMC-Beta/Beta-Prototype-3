using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform aliaTrans;
    [SerializeField] float rotateSpeed = 3f;

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, aliaTrans.eulerAngles.y, 0);
        //transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, new Vector3(0, aliaTrans.eulerAngles.y, 0) , Time.deltaTime * rotateSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, -1, 0, aliaTrans.eulerAngles.y * Mathf.Deg2Rad), Time.deltaTime * rotateSpeed);
    }
}
