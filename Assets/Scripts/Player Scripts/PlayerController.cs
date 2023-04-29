using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float maxSpeed;
    public Rigidbody2D playerRB;
    private Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = 10f;
    }

    // Update is called once per frame
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
