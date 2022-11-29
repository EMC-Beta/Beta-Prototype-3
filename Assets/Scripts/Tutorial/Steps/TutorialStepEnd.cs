using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FinishStep", menuName = "Tutorial/FinishStep")]
public class TutorialStepEnd : TutorialStepBase
{
    public override void StartStep()
    {
        //Default text
        //if (description == "")
        //{
        //    description = "Great Job!";
        //}
    }

    public override bool UpdateStep(TutorialManager manager)
    {
        //End step once audio is done playing, then tutorial is over
        if(!manager.audioSource.isPlaying)
        {
            return true;
        }

        return false;
    }

    public override void UpdateSlider(Slider slider)
    {
        //Show slider should be false so this shouldn't get called
    }
}
