using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Localization.Settings;

public abstract class TutorialStepBase : ScriptableObject
{
    //public string description = ""; //Text to put on checklist
    public AudioClip audio;         //Audio to play at this step
    public Sprite pictograph;       //Image to display

    public bool showAudio = true;
    public bool showProgress = true;

    public string localeKey = "";

    //To be called when first arriving at step
    public abstract void StartStep();

    //Define the conditions that satisfy this tutorial step, return true if satisfied
    public abstract bool UpdateStep(TutorialManager manager);

    //Update progress slider according to this rule
    public abstract void UpdateSlider(Slider slider);
}
