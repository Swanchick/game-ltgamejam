using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuActions : MonoBehaviour
{
    [SerializeField]
    public SettingsData settings;

    public TMP_Dropdown languageDropdown;
    public Toggle highQuality;
    public Toggle lowQuality;

    public static SettingsMenuActions Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadGraphicsQuality();
        highQuality.onValueChanged.AddListener(delegate { HighQualityListener(); });
        lowQuality.onValueChanged.AddListener(delegate { LowQualityListener(); });
    }

    private void HighQualityListener()
    {
        if (highQuality.isOn)
        {
            HighQualityChanged();
        }
    }

    private void HighQualityChanged()
    {
        SetGraphicsQuality(1);
        lowQuality.isOn = false;
        highQuality.isOn = true;
        settings.GraphicsQuality = SettingsData.GraphicsQualityEnum.High;
    }

    private void LowQualityListener()
    {
        if (lowQuality.isOn)
        {
            LowQualityChanged();
        }
    }

    private void LowQualityChanged()
    {
        SetGraphicsQuality(0);
        highQuality.isOn = false;
        lowQuality.isOn = true;
        settings.GraphicsQuality = SettingsData.GraphicsQualityEnum.Low;
    }

    public void SetGraphicsQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    public void LoadGraphicsQuality()
    {
        print(settings.GraphicsQuality);
        if (settings.GraphicsQuality == SettingsData.GraphicsQualityEnum.High)
        {
            HighQualityChanged();
        } else
        {
            LowQualityChanged();
        }
    }
}
