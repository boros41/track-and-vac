using GolemKinGames.Vacumn;
using UnityEngine;

public class CheatCodeManager : MonoBehaviour
{
    private static VacuumSystem _vacuum;
    [SerializeField] private BatteryBarUI _batteryBar;

    [SerializeField] private static Timer _timer;

    public static void SetVacuumInstance(VacuumSystem vacuum)
    {
        _vacuum = vacuum;
    }

    public static void SetTimerInstance(Timer timer)
    {
        _timer = timer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_vacuum == null) return;


        if (BatteryCheatPressed())
        {
            _vacuum.SetFullBattery();
            UpdateBatteryUI();
        }

        if (IsAddTimeCheatPressed())
        {
            print($"Add time cheat pressed.");

            const int ONE_MINUTE = 60;

            UpdateRemainingTime(ONE_MINUTE);
        }

        if (IsRemoveTimeCheatPressed())
        {
            print($"Remove time cheat pressed.");

            const int ONE_MINUTE = -60;

            UpdateRemainingTime(ONE_MINUTE);
        }

        if (IsCleanAllItemsPressed())
        {
            print("Cleaning all items cheat pressed.");

            CleanAllItems();
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

    private static bool IsAddTimeCheatPressed()
    {
        return Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.T) && Input.GetKeyDown(KeyCode.Equals);
    }

    private static bool IsRemoveTimeCheatPressed()
    {
        return Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.T) && Input.GetKeyDown(KeyCode.Minus);
    }

    private static void UpdateRemainingTime(int seconds)
    {
        print($"Updating time");

        Timer.Instance.RemainingTime += seconds;
    }

    private static bool IsCleanAllItemsPressed()
    {

        return Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.X);
    }

    private static void CleanAllItems()
    {
        print("Cleaning all items.");
        CleanablesTracker.Instance.Cleanables.Clear();
    }

}
