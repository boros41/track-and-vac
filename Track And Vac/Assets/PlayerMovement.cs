using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _movementSpeed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float forwardMovement = Input.GetAxis("Vertical");

        // Moves based on the direction the player is facing (local). Using new Vector3() would move regardless of the player's direction.
        UnityEngine.Vector3 move = transform.right * horizontalMovement + transform.forward * forwardMovement;

        _controller.Move(_movementSpeed * Time.deltaTime * move);
    }
}
