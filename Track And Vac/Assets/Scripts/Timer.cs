using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private float _remainingTime;

    public float RemainingTime
    {
        get => _remainingTime;
        set
        {
            if (value < 0) _remainingTime = 0;

            // TODO: Set a limit to remaining time to avoid overflow maybe 
            _remainingTime = value; 
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CountDownTimer();
    }

    private void CountDownTimer()
    {

        if (IsTimeNotOver())
        {
            RemainingTime -= Time.deltaTime;
        }
        else
        {
            RestartGame();
        }

        UpdateRemainingTime();
    }

    private bool IsTimeNotOver()
    {
        return RemainingTime > 0;
    }

    private void RestartGame()
    {
        RemainingTime = 0;

        string mainScene = SceneManager.GetSceneByName("MainScene").name;

        SceneManager.LoadScene(mainScene);
    }

    private void UpdateRemainingTime()
    {
        int minutes = Mathf.FloorToInt(RemainingTime / 60);
        int seconds = Mathf.FloorToInt(RemainingTime % 60);
        _timer.text = $"{minutes:00}:{seconds:00}";
    }
}
