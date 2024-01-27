using UnityEngine;

[CreateAssetMenu(fileName = "NewSettingsData", menuName = "SettingsData")]
public class SettingsData : ScriptableObject
{
    public enum GraphicsQualityEnum
    {
        High,
        Low
    }

    public GraphicsQualityEnum GraphicsQuality;
    public float MusicVolume;
    public float SFXVolume;
    public float FOVValue;
}
