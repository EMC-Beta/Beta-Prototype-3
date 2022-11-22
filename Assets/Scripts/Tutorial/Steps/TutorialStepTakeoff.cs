using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="TakeoffStep", menuName = "Tutorial/TakeoffStep")]
public class TutorialStepTakeoff : TutorialStepBase
{
    public override void StartStep()
    {
        //Default text
        //if (description == "")
        //{
        //    description = "Press 'Space' to take off!";
        //}
    }

    public override bool UpdateStep(TutorialManager manager)
    {
        if(Input.GetKey(KeyCode.Space))
        {
            return true;
        }

        return false;
    }

    public override void UpdateSlider(Slider slider)
    {
        //showSlider will be false so this shouldn't get called
    }
}
