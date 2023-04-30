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
        shouldAccelerate = false;
        _lockAllActions = false;
    }

    void Update()
    {
        if (_lockAllActions) return;

        _throw.HandleThrowingInput();
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


    private void UpdateSpeed()
    {
        if (shouldAccelerate) currentSpeed += acceleration;
        else currentSpeed -= deceleration;
        currentSpeed = Mathf.Max(Mathf.Min(maxSpeed, currentSpeed), 0);

        moveVector = lastMoveDir * Time.fixedDeltaTime * currentSpeed;

    }
    void Move()
    {
        //playerRB.MovePosition(transform.position + new Vector3(moveDir.x * currentSpeed * Time.fixedDeltaTime, moveDir.y * currentSpeed * Time.fixedDeltaTime, 0));
        playerRB.MovePosition(transform.position + moveVector);
    }
}
