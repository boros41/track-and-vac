using GolemKinGames.Vacumn;
using UnityEngine;

public class CheatCodeManager : MonoBehaviour
{
    private static VacuumSystem _vacuum;
    [SerializeField] private BatteryBarUI _batteryBar;

    public static void SetVacuumInstance(VacuumSystem vacuum)
    {
        _vacuum = vacuum;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_vacuum != null && BatteryCheatPressed())
        {
            _vacuum.SetFullBattery();
            UpdateBatteryUI();
        }
    }

    private void UpdateBatteryUI()
    {
        _batteryBar.MaxBattery = 100;
        _batteryBar.Battery = 100;
    }

    private static bool BatteryCheatPressed()
    {
        return Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B);
    }


}
