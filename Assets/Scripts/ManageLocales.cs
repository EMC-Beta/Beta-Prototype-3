using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ManageLocales : MonoBehaviour
{
    bool localizationActive = false;

    //I wanted to make a Locales enum for the parameter, but button OnClick listeners don't support enum parameters
    public void ChangeLocale(int localeId)
    {
        if (localizationActive)
        {
            return;
        }
        StartCoroutine(SetLocale(localeId));
    }

    IEnumerator SetLocale(int localeId)
    {
        localizationActive = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        localizationActive = false;
    }
}
