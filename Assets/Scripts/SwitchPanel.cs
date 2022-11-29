using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPanel : MonoBehaviour
{
    GameObject from, to;
    public void SetFrom(GameObject _from)
    {
        from = _from;
    }

    public void SetTo(GameObject _to)
    {
        to = _to;
    }

    public void ChangePanel()
    {
        from.SetActive(false);
        to.SetActive(true);
    }
}
