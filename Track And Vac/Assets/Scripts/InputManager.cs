using GolemKinGames.Vacumn;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnableVacuum()
    {
        const KeyCode LEFT_MOUSE_CLICK = KeyCode.Mouse0;
        bool isVacuumOn = Input.GetKeyDown(LEFT_MOUSE_CLICK) ? true : false;

        if (isVacuumOn)
        {
            
        }
    }
}
