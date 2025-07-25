using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMech : MonoBehaviour 
{

	public Vector3 OpenRotation, CloseRotation;

	public float rotSpeed = 1f;

	public bool doorBool;


	void Start()
	{
		doorBool = false;
	}
		
	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.tag == ("Player") && Input.GetKeyDown(KeyCode.E))
		{
            Debug.Log("Player is in trigger zone");
            Debug.Log("E pressed");
            if (!doorBool)
				doorBool = true;
			else
				doorBool = false;

			print(doorBool);
		}
	}

	void Update()
	{
		if (doorBool)
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (OpenRotation), rotSpeed * Time.deltaTime);
		else
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (CloseRotation), rotSpeed * Time.deltaTime);	
	}

}

