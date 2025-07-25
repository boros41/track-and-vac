using UnityEngine;

public class VacuumSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource vacuumSoundInstance;
    [SerializeField] private AudioSource vacuumOffSoundInstance;
    [SerializeField] private AudioSource pickUpSoundInstance;


    public static AudioSource vacuumSound;
    public static AudioSource vacuumOffSound;
    public static AudioSource pickUpSound;

    private void Awake()
    {
        vacuumSound = vacuumSoundInstance;
        vacuumOffSound = vacuumOffSoundInstance;
        pickUpSound = pickUpSoundInstance;
    }
}
