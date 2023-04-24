using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController3D : MonoBehaviour
{
    //  Movement
    private float horizontalInput, forwardInput, verticalInput;
    private float desiredX;
    private float speedMultiplier;
    private bool lockMovement;
    public float _normalMovementForce;
    public float _currentMovementForce;
    public float _maxMoveSpeed;
    public float _counterMovementModifier;

    // Dash
    public float _timeSinceLastDash;
    public float _dashCooldown;
    public float _dashForceMultiplier;

    // Physics

    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;
    private Vector3 normalVector = Vector3.up;
    private bool cancellingGrounded;
    public float _floatingDrag;
    public float _walkingDrag;

    //  Look
    [HideInInspector] public float _tempSensitivity;
    [HideInInspector] public float _currentSensitivity;
    [HideInInspector] public float _maxSensitivity;
    public static float _savedSens = -1;
    private float xRotation;
    public bool _lockRotation;
    public Transform _orientation;

    //  Jump/Grounded

    [HideInInspector] public float jumpForce;
    [HideInInspector] public float jumpCooldown;
    private bool jumping;
    private bool readyToJump;
    public bool _grounded;
    public LayerMask whatIsGround;

    //  Misc

    [HideInInspector] public KeyCode _lastHitKey;
    [HideInInspector] public Coroutine _dragCoroutine;
    [HideInInspector] public Rigidbody _playerRB;
    [HideInInspector] public Collider _playerCol;
    [HideInInspector] public Camera _playerCam;
    public Camera _FPSLayerCam;
    [HideInInspector] public bool _firstPersonActive;
    public float _totalDamageDealtByPlayer;

    public void Awake()
    {
        transform.tag = "Player";
        _playerCam = Camera.main;
        _playerRB = GetComponentInChildren<Rigidbody>();
        _playerCol = GetComponentInChildren<Collider>();
    }
    public void Start()
    {
        _currentMovementForce = _normalMovementForce;

        _firstPersonActive = true;
        readyToJump = true;
        _playerCol.isTrigger = false;
        _playerRB.freezeRotation = true;
        lockMovement = _lockRotation = false;
        _playerCol.isTrigger = false;

        _timeSinceLastDash = _dashCooldown;
        speedMultiplier = 1;

        _totalDamageDealtByPlayer = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void AddTotalDamage(float dmg)
    {
        _totalDamageDealtByPlayer += dmg;
    }

    public void Update()
    {
        HandleKeyboardInput();
        Look();
        _timeSinceLastDash += Time.deltaTime;
    }
    public void FixedUpdate()
    {
        if (!lockMovement) HandleMovement();
    }
    //  Input

    private void HandleKeyboardInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        forwardInput = Input.GetAxisRaw("Vertical");
        verticalInput = 0;
        if (Input.GetKey(KeyCode.Space)) verticalInput = 1;
        if (Input.GetKey(KeyCode.C)) verticalInput = -1;

        jumping = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.LeftShift) && _timeSinceLastDash > _dashCooldown)
        {
            if (Input.GetKey(KeyCode.W)) Dash(0);
            if (Input.GetKey(KeyCode.A)) Dash(1);
            if (Input.GetKey(KeyCode.S)) Dash(2);
            if (Input.GetKey(KeyCode.D)) Dash(3);
            if (Input.GetKey(KeyCode.Space)) Dash(4);
            if (Input.GetKey(KeyCode.C)) Dash(5);
        }
    }

    private void Dash(int direction)
    {
        _playerRB.velocity = Vector3.zero;
        if (direction == 0) _playerRB.AddForce(_playerCam.transform.forward * _dashForceMultiplier * _currentMovementForce); // W
        if (direction == 1) _playerRB.AddForce(_playerCam.transform.right * -1 * _dashForceMultiplier * _currentMovementForce); // A
        if (direction == 2) _playerRB.AddForce(_playerCam.transform.forward * -1 * _dashForceMultiplier * _currentMovementForce); // S
        if (direction == 3) _playerRB.AddForce(_playerCam.transform.right * _dashForceMultiplier * _currentMovementForce); // D
        if (direction == 4) _playerRB.AddForce(transform.up * _dashForceMultiplier * _currentMovementForce); // Space = Swim Up
        if (direction == 5) _playerRB.AddForce(transform.up * -1 * _dashForceMultiplier * _currentMovementForce); // C = Swim Down
        _timeSinceLastDash = 0;
    }

    private void Look()
    {
        if (_lockRotation) return;

        float mouseX = Input.GetAxis("Mouse X") * _currentSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _currentSensitivity * Time.fixedDeltaTime;

        //Find current look rotation
        Vector3 rot = _playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        _playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        _FPSLayerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        _orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    // Movement

    private void HandleMovement()
    {
        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(horizontalInput, forwardInput, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed = _maxMoveSpeed * speedMultiplier;

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (horizontalInput > 0 && xMag > maxSpeed * speedMultiplier) horizontalInput = 0;
        if (horizontalInput < 0 && xMag < -maxSpeed * speedMultiplier) horizontalInput = 0;
        if (forwardInput > 0 && yMag > maxSpeed * speedMultiplier) forwardInput = 0;
        if (forwardInput < 0 && yMag < -maxSpeed * speedMultiplier) forwardInput = 0;

        if (_playerRB.velocity.magnitude > maxSpeed * speedMultiplier)
        {
            horizontalInput = 0;
            forwardInput = 0;
            verticalInput = 0;
        }

        //Apply forces to move player
        if (!_grounded) //if floating, move towards camera, otherwise, try walking on the seabed
        {
            _playerRB.useGravity = false;
            _playerRB.drag = _floatingDrag;
            _playerRB.AddForce((
                _playerCam.transform.forward * forwardInput
                + Vector3.up * verticalInput
                + _playerCam.transform.right * horizontalInput)
                * _currentMovementForce * Time.deltaTime * speedMultiplier);
        }
        else
        {
            _playerRB.useGravity = true;
            _playerRB.drag = _walkingDrag;
            _playerRB.AddForce(_orientation.transform.forward * forwardInput * _currentMovementForce * Time.deltaTime * speedMultiplier);
            _playerRB.AddForce(_orientation.transform.right * horizontalInput * _currentMovementForce * Time.deltaTime * speedMultiplier);
        }
    }
    private void Jump()
    {
        if (_grounded && readyToJump)
        {
            readyToJump = false;

            //Add jump forces
            _playerRB.AddForce(Vector2.up * jumpForce * 1.5f);
            _playerRB.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = _playerRB.velocity;
            if (_playerRB.velocity.y < 0.5f)
                _playerRB.velocity = new Vector3(vel.x, 0, vel.z);
            else if (_playerRB.velocity.y > 0)
                _playerRB.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!_grounded || jumping) return;
        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            _playerRB.AddForce(_currentMovementForce * _orientation.transform.right * Time.deltaTime * -mag.x * _counterMovementModifier);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            _playerRB.AddForce(_currentMovementForce * _orientation.transform.forward * Time.deltaTime * -mag.y * _counterMovementModifier);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(_playerRB.velocity.x, 2) + Mathf.Pow(_playerRB.velocity.z, 2))) > _maxMoveSpeed)
        {
            float fallspeed = _playerRB.velocity.y;
            Vector3 n = _playerRB.velocity.normalized * _maxMoveSpeed;
            _playerRB.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    //  Grounded
    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                _grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }
    private void StopGrounded()
    {
        _grounded = false;
    }


    //  Misc

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = _orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(_playerRB.velocity.x, _playerRB.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = _playerRB.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
    private void OnGUI()
    {
        if (Event.current.isKey)
        {
            if (Event.current.keyCode != KeyCode.None) _lastHitKey = Event.current.keyCode;
        }
    }
}