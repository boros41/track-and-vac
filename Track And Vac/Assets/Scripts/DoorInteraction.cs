using System.Collections;
using TMPro;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private TextMeshProUGUI _interactText;

    public Vector3 OpenRotation, CloseRotation;

    public float rotSpeed = 1f;

    public bool doorBool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorBool = false;
    }
    /*void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == ("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player is in trigger zone");
            Debug.Log("E pressed");
            if (!doorBool)
                doorBool = true;
            else
                doorBool = false;

            print(doorBool);
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        if (doorBool)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(OpenRotation), rotSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(CloseRotation), rotSpeed * Time.deltaTime);
        }
    }

    public void Interact()
    {
        if (!doorBool)
        {
            doorBool = true;
            print("Door opened");
        }
        else
        {
            doorBool = false;
            print("Door closed");
        }
    }

    public string GetDescription()
    {
        return "Open Door";
    }
}
