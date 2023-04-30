using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRB;
    [HideInInspector] public PackageThrowing _throw;
    private Animator anim;

    public bool _lockAllActions;

    //  Movement
    private float currentSpeed;
    private float maxSpeed;
    private Vector3 moveDir;
    private Vector3 moveVector;
    private Vector3 lastMoveDir;
    private float acceleration;
    private float deceleration;
    private bool shouldAccelerate;

    //  Audio
    public AudioSource _footstepsSound;
    public bool _footstepsPlaying;

    //  Raycast Hits
    public LayerMask mask;
    private RaycastHit2D leftRaycastHit;
    private RaycastHit2D upRaycastHit;
    private RaycastHit2D downRaycastHit;
    private RaycastHit2D rightRaycastHit;
    private float minDistanceToWall;

    private void Awake()
    {
        REF.pCon = this;
        anim = GetComponentInChildren<Animator>();
        _throw = GetComponentInChildren<PackageThrowing>();

        moveVector = lastMoveDir = moveDir = Vector2.zero;
    }
    void Start()
    {
        maxSpeed = 12f;
        acceleration = 0.75f;
        deceleration = 2 * acceleration;
        minDistanceToWall = 0.75f;
        shouldAccelerate = false;
        _lockAllActions = false;
    }

    void Update()
    {
        if (_lockAllActions) return;

        _throw.HandleThrowingInput();
        UpdateRaycasts();
        ProcessInputs();
    }

    void FixedUpdate()
    {
        if (_lockAllActions) return;

        _throw.ThrowPackage();
        UpdateSpeed();
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX < 0 && leftRaycastHit.distance < minDistanceToWall)
        {
            moveX = 0;
        }
        if (moveX > 0 && rightRaycastHit.distance < minDistanceToWall)
        {
            moveX = 0;
        }
        if (moveY < 0 && downRaycastHit.distance < minDistanceToWall)
        {
            moveY = 0;
        }
        if (moveY > 0 && upRaycastHit.distance < minDistanceToWall)
        {
            moveY = 0;
        }

        moveDir = new Vector3(moveX, moveY, 0);
        moveDir.Normalize();

        if ((HM.OppositeSignsFloat(moveX, lastMoveDir.x) && moveY == 0) ||
            (HM.OppositeSignsFloat(moveY, lastMoveDir.y) && moveX == 0) ||
            (HM.OppositeSignsFloat(moveY, lastMoveDir.y) && HM.OppositeSignsFloat(moveX, lastMoveDir.x)))
        {
            currentSpeed = 0;
            Debug.Log("full stop");
        }

        if (moveDir.magnitude != 0) shouldAccelerate = true;
        else shouldAccelerate = false;

        if (moveDir.magnitude > 0) lastMoveDir = moveDir;
        anim.SetFloat("Speed", currentSpeed);
        anim.SetFloat("Horizontal", moveX);
        anim.SetFloat("Vertical", moveY);
    }

    private void UpdateRaycasts()
    {
        leftRaycastHit = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, mask);
        rightRaycastHit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, mask);
        upRaycastHit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, mask);
        downRaycastHit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, mask);

        Debug.DrawRay(transform.position, Vector2.left * leftRaycastHit.distance, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * rightRaycastHit.distance, Color.red);
        Debug.DrawRay(transform.position, Vector2.up * upRaycastHit.distance, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * downRaycastHit.distance, Color.red);
    }


    private void UpdateSpeed()
    {
        if (shouldAccelerate) currentSpeed += acceleration;
        else currentSpeed -= deceleration;
        currentSpeed = Mathf.Max(Mathf.Min(maxSpeed, currentSpeed), 0);
        moveVector = lastMoveDir * Time.fixedDeltaTime * currentSpeed;

        if (moveVector.magnitude > 0)
        {
            if (_footstepsPlaying) return;
            _footstepsSound.Play();
            _footstepsPlaying = true;
        }
        else
        {
            _footstepsSound.Stop();
            _footstepsPlaying = false;
        }
    }
    void Move()
    {
        //playerRB.MovePosition(transform.position + new Vector3(moveDir.x * currentSpeed * Time.fixedDeltaTime, moveDir.y * currentSpeed * Time.fixedDeltaTime, 0));
        playerRB.MovePosition(transform.position + moveVector);
    }
}
