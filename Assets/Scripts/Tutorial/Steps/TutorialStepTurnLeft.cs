using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TurnLeftStep", menuName = "Tutorial/TurnLeftStep")]
public class TutorialStepTurnLeft : TutorialStepBase
{
    [SerializeField] float maxHoldTime = 1f;
    [SerializeField] float holdTime = 1f;

    public override void StartStep()
    {
        holdTime = maxHoldTime;
        //Default text
        //if (description == "")
        //{
        //    description = "Press 'Q' to turn left";
        //}
    }

    public override bool UpdateStep(TutorialManager manager)
    {
        if (Input.GetKey(KeyCode.Q))
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
