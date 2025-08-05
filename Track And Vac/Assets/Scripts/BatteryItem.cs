using GolemKinGames.Vacumn;
using TMPro;
using UnityEngine;

public class BatteryItem : MonoBehaviour, IInteractable
{
    [SerializeField] private TextMeshProUGUI _interactText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        print("Battery collected");

        VacuumSystem.Instance.BatteriesLeft += 1;

        Destroy(gameObject);
    }

    public string GetDescription()
    {
        return "Collect Battery";
    }
}
