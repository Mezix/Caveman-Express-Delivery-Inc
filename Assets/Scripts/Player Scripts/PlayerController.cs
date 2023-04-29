using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float maxSpeed;
    public Rigidbody2D playerRB;
    private Vector2 moveDirection;
    public PackageThrowing _throw;

    public bool _lockAllActions;

    private void Awake()
    {
        REF.pCon = this;
        _throw = GetComponentInChildren<PackageThrowing>();
    }
    void Start()
    {
        maxSpeed = 10f;
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
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY);
    }

    void Move()
    {
        playerRB.MovePosition(transform.position + new Vector3(moveDirection.x * maxSpeed * Time.fixedDeltaTime, moveDirection.y * maxSpeed * Time.fixedDeltaTime, 0));
    }
}
