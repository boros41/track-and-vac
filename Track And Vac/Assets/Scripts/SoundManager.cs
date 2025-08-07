using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class SoundManager : MonoBehaviour
{
    public enum Sound
    {
        SPEED_BUFF_PICKUP,
        BATTERY_PICKUP,
        DOOR_OPEN,
        DOOR_CLOSE,
    }

    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource _speedBuffSound;
    [SerializeField] private AudioSource _batteryPickUpSound;

    //public AudioSource SpeedBuffSound => _speedBuffSound;
    //public AudioSource BatteryPickUpSound => _batteryPickUpSound;

    [SerializeField] private List<SoundEntry> _sounds;
    public static Dictionary<Sound, AudioSource> SoundToSource;




    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            SoundToSource = new Dictionary<Sound, AudioSource>();
            InitializeDictionary(SoundToSource, _sounds);
        } else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private static void InitializeDictionary(Dictionary<Sound, AudioSource> soundToSource, List<SoundEntry> sounds)
    {
        foreach (SoundEntry currentSoundEntry in sounds)
        {
            if (soundToSource.ContainsKey(currentSoundEntry.Key)) continue;
                
            soundToSource.Add(currentSoundEntry.Key, currentSoundEntry.Sound);
        }
    }

    [System.Serializable]
    internal class SoundEntry
    {
        [SerializeField] public Sound Key;
        [SerializeField] public AudioSource Sound;
    }
}
