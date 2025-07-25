using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform batteryBar;
    [SerializeField] private TextMeshProUGUI batteryBarText;

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
}
