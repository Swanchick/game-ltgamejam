using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class FOVSettings : MonoBehaviour
{
    [SerializeField]
    public SettingsData settings;

    [SerializeField] private Slider FOVSlider;

    private void Start()
    {
        if (settings)
        {
            LoadValue();
        }
        else
        {
            SetFOVValue();
        }
    }
    public void SetFOVValue()
    {
        float value = FOVSlider.value;
        settings.FOVValue = value;
    }

    private void LoadValue()
    {
        FOVSlider.value = settings.FOVValue;
        SetFOVValue();
    }
}
