using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ForwardStep", menuName = "Tutorial/ForwardStep")]
public class TutorialStepForward : TutorialStepBase
{
    [SerializeField] float maxHoldTime = 1f;
    [SerializeField] float holdTime = 1f;

    public override void StartStep()
    {
        holdTime = maxHoldTime;
        //Default text
        if (description == "")
        {
            description = "Press 'W' to move forward";
        }
    }

    public override bool UpdateStep(TutorialManager manager)
    {
        if (Input.GetKey(KeyCode.W))
        {
            holdTime -= Time.deltaTime;
        }

        if(holdTime <= 0)
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
