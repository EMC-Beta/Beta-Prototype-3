using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Localization.Settings;

[CreateAssetMenu(fileName = "BackStep", menuName = "Tutorial/BackStep")]
public class TutorialStepBack : TutorialStepBase
{
    [SerializeField] float maxHoldTime = 1f;
    [SerializeField] float holdTime = 1f;

    public override void StartStep()
    {
        holdTime = maxHoldTime;
        //Default text
        /*if (description == "")
        {
            description = "Press 'S' to move backward";
        }*/
    }

    public override bool UpdateStep(TutorialManager manager)
    {
        if (Input.GetKey(KeyCode.S))
        {
            holdTime -= Time.deltaTime;
        }

        if (holdTime <= 0)
        {
            return true;
        }

        return false;
    }

    public override void UpdateSlider(Slider slider)
    {
        slider.maxValue = maxHoldTime;
        slider.value = holdTime;
    }
}
