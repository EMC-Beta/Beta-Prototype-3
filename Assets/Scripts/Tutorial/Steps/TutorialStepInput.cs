using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "InputStep", menuName = "Tutorial/InputStep")]
public class TutorialStepInput : TutorialStepBase
{
    [Header("Hold")]
    [SerializeField] float maxHoldTime = 1f;
    [SerializeField] float holdTime = 1f;
    [SerializeField] bool holdKey = false;

    [Header("Input")]
    [SerializeField] KeyCode key;

    public override void StartStep()
    {
        holdTime = maxHoldTime;
        //Default text
        //if (description == "")
        //{
        //    description = "Press 'Space' to take off!";
        //}
    }

    public override bool UpdateStep(TutorialManager manager)
    {
        if (Input.GetKey(key))
        {
            if(holdKey)
            {
                holdTime -= Time.deltaTime;

                if (holdTime <= 0)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public override void UpdateSlider(Slider slider)
    {
        if(showProgress)
        {
            slider.maxValue = maxHoldTime;
            slider.value = holdTime;
        }
    }
}
