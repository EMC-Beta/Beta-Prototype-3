using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TurnRightStep", menuName = "Tutorial/TurnRightStep")]

public class TutorialStepTurnRight : TutorialStepBase
{
    [SerializeField] float maxHoldTime = 1f;
    [SerializeField] float holdTime = 1f;

    public override void StartStep()
    {
        holdTime = maxHoldTime;
        //Default text
        if (description == "")
        {
            description = "Press 'E' to turn right";
        }
    }

    public override bool UpdateStep(TutorialManager manager)
    {
        if (Input.GetKey(KeyCode.E))
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
