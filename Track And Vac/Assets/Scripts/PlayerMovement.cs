using System;
using System.Collections;
using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    [SerializeField] private CharacterController _controller;

    private float _stepTimer = 0f;
    private const float STEP_INTERVAL = .65f;
    private float _movementSpeed = 2f;
    public float MovementSpeed
    {
        get => _movementSpeed;
        set
        {
            if (value is <= 0 or > 4)
            {
                throw new ArgumentException($"Movement speed out of range [1,4]");
            }

            _movementSpeed = value;
        }
    }
    [SerializeField] private Animator _animator;
    private static readonly int isMovingId = Animator.StringToHash("isMoving");

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
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
        Move();
    }

    private void Move()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float forwardMovement = Input.GetAxis("Vertical");

        // Moves based on the direction the player is facing (altered by MouseLook.cs). Using new Vector3() would move regardless of the player's direction.
        UnityEngine.Vector3 movement = (transform.right * horizontalMovement + transform.forward * forwardMovement) * (MovementSpeed * Time.deltaTime);

        if (IsMoving(movement))
        {
            _stepTimer -= Time.deltaTime;

            /*if (!SoundManager.SoundToSource[SoundManager.Sound.FOOTSTEP].isPlaying)
            {
                SoundManager.SoundToSource[SoundManager.Sound.FOOTSTEP].Play();
            }*/

            if (_stepTimer <= 0f)
            {
                SoundManager.SoundToSource[SoundManager.Sound.FOOTSTEP].Play();
                _stepTimer = STEP_INTERVAL;
            }

            _controller.Move(movement);
            _animator.SetBool(isMovingId, true);
        }
        else
        {
            if (SoundManager.SoundToSource[SoundManager.Sound.FOOTSTEP].isPlaying)
            {
                SoundManager.SoundToSource[SoundManager.Sound.FOOTSTEP].Stop();
                _stepTimer = 0f;
            }

            _animator.SetBool(isMovingId, false);
        }

    }

    private static bool IsMoving(UnityEngine.Vector3 movement)
    {
        return movement != UnityEngine.Vector3.zero;
    }
}
