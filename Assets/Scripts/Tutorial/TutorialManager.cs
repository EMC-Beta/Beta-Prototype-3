using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public AudioSource audioSource;
    LocalizeStringEvent textEvent;      //The component where we can change the key that corresponds to a string in a Locale table
    Image stepImage;
    Slider progressSlider;

    [SerializeField] TutorialStepBase[] steps;
    uint stepIndex = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        textEvent = transform.Find("DescriptionText").GetComponent<LocalizeStringEvent>();
        stepImage = transform.Find("Pictograph").GetComponent<Image>();
        progressSlider = transform.Find("Progress").GetComponent<Slider>();
        ChangeStep();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void Update()
    {
        //If at end of tutorial, disable checklist
        if(stepIndex < steps.Length)
        {
            steps[stepIndex].UpdateSlider(progressSlider);

            //Check if goal fulfilled
            if (steps[stepIndex].UpdateStep(this))
            {
                //Increment steps and switch to new step if there is one
                stepIndex++;
                if (stepIndex < steps.Length)
                {
                    ChangeStep();
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void ChangeStep()
    {
        TutorialStepBase currentStep = steps[stepIndex];

        //Set up the current step
        currentStep.StartStep();

        //Set description in checklist
        //stepText.text = currentStep.description;
        //textEvent.StringReference = "TutorialTable/" + ;
        textEvent.SetEntry(currentStep.localeKey);

        //Play audio
        if (currentStep.showAudio)
        {
            PlaySound(currentStep.audio);
        }

        if(currentStep.showProgress)
        {
            progressSlider.gameObject.SetActive(true);
        }
        else
        {
            progressSlider.gameObject.SetActive(false);
        }

        //Set image in checklist
        stepImage.sprite = currentStep.pictograph;
    }
}
