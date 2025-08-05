using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class BatteryBarUI : MonoBehaviour
{
    public static BatteryBarUI Instance { get; private set; }
    [SerializeField] private RectTransform batteryBar;
    [SerializeField] private TextMeshProUGUI batteryBarText;
    [SerializeField] private TextMeshProUGUI batteriesLeftText;

    //public float Battery;
    //public float MaxBattery;
    public float Width;
    public float Height;

    public float MaxBattery { get; set; }

    private float _battery;
    public float Battery
    {
        get => _battery;
        set
        {
            _battery = value;

            batteryBarText.SetText($"{_battery:F0}%");

            float newWidth = (Battery / MaxBattery) * Width;

            batteryBar.sizeDelta = new Vector2(newWidth, Height);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void SetBatteriesLeftText(int batteries)
    {
        switch (batteries)
        {
            case 0:
                batteriesLeftText.SetText("");
                break;
            case 1:
                batteriesLeftText.SetText("ϟ");
                break;
            case 2:
                batteriesLeftText.SetText("ϟ ϟ");
                break;
            case 3:
                batteriesLeftText.SetText("ϟ ϟ ϟ");
                break;
        }

    }
}
