using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nw : MonoBehaviour
{
    
    public SettingsData settings;
    public void ShowData()
    {
        print(settings.MusicVolume);
        print(settings.SFXVolume);
        print(settings.GraphicsQuality);
    }
}
