using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float maxSpeed;
    public Rigidbody2D playerRB;
    private Vector2 moveDirection;
    [HideInInspector] public PackageThrowing _throw;
    private Animator anim;

    public bool _lockAllActions;

    private void Awake()
    {
        REF.pCon = this;
        anim = GetComponentInChildren<Animator>();
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

        anim.SetFloat("Speed", moveDirection.magnitude);
        anim.SetFloat("Horizontal", moveX);
        anim.SetFloat("Vertical", moveY);
    }

    void Move()
    {
        playerRB.MovePosition(transform.position + new Vector3(moveDirection.x * maxSpeed * Time.fixedDeltaTime, moveDirection.y * maxSpeed * Time.fixedDeltaTime, 0));
    }
}
