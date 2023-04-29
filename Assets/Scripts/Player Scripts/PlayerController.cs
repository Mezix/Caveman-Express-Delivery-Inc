using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float maxSpeed;
    public Rigidbody2D playerRB;
    private Vector2 moveDirection;
    public PackageThrowing _throw;

    private void Awake()
    {
        REF.pCon = this;
        _throw = GetComponentInChildren<PackageThrowing>();
    }
    void Start()
    {
        maxSpeed = 10f;
    }

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
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
