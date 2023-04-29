using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float maxSpeed;
    public Rigidbody2D playerRB;
    private Vector2 moveDirection;

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
        playerRB.velocity = new Vector2(moveDirection.x * maxSpeed, moveDirection.y * maxSpeed);
    }
}
