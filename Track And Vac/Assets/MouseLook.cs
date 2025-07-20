using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private readonly float _mouseSensitivity = 200f;
    [SerializeField] private Transform _player;
    private float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MouseMovement();
    }

    private void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; // positive = down, negative = up
        xRotation = Math.Clamp(xRotation, -90f, 60f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        _player.Rotate(Vector3.up * mouseX);
    }
}
