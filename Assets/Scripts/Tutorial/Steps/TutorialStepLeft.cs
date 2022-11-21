using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LeftStep", menuName = "Tutorial/LeftStep")]
public class TutorialStepLeft : TutorialStepBase
{
    [SerializeField] float maxHoldTime = 1f;
    [SerializeField] float holdTime = 1f;

    public override void StartStep()
    {
        holdTime = maxHoldTime;
        //Default text
        if (description == "")
        {
            description = "Press 'A' to move left";
        }
    }

    public override bool UpdateStep(TutorialManager manager)
    {
        if (Input.GetKey(KeyCode.A))
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
