using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CleanablesTracker : MonoBehaviour
{
    public static CleanablesTracker Instance { get; private set; }

    public List<GameObject> Cleanables { get; private set; }

    [SerializeField] private TextMeshProUGUI _cleanablesText;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cleanables = GameObject.FindGameObjectsWithTag("Cleanable").ToList();

        UpdateCleanablesText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCleanablesText();
    }

    private void UpdateCleanablesText()
    {
        _cleanablesText.SetText($"Items left: {Cleanables.Count}");
    }
}
