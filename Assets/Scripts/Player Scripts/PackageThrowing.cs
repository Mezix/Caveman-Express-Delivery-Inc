using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PackageThrowing : MonoBehaviour
{
    [HideInInspector] public float loadingSpeed;
    [HideInInspector] public float currentThrowForce;
    [HideInInspector] public float maxThrowingSpeed;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public ThrowingState throwingState;

    [HideInInspector] public GameObject nextPackage;

    [HideInInspector] public float currentCooldown;
    [HideInInspector] public float throwCooldown;
    [HideInInspector] public float startThrowForce;

    //  Audio
    public AudioSource _throwSound;

    public enum ThrowingState
    {
        PackageInHand,
        Aiming,
        Throw,
        OnCooldown
    }

    void Awake()
    {
        nextPackage = (GameObject)Resources.Load(GS.Prefabs("Package"));
    }
    void Start()
    {
        loadingSpeed = 0.4f;
        maxThrowingSpeed = 50f;
        throwingState = ThrowingState.PackageInHand;
        startThrowForce = 5f;
        currentThrowForce = startThrowForce;
        currentCooldown = 0;
        throwCooldown = 25f;
    }

    public void ThrowPackage()
    {
        if (throwingState == ThrowingState.Aiming)
        {
            currentThrowForce = Mathf.Min(maxThrowingSpeed, currentThrowForce + loadingSpeed);
        }
        else if (throwingState == ThrowingState.Throw)
        {
            Throw();
            currentCooldown = throwCooldown;
            throwingState = ThrowingState.OnCooldown;
        }
        else if (throwingState == ThrowingState.OnCooldown)
        {
            currentCooldown--;
            if (currentCooldown <= 0) throwingState = ThrowingState.PackageInHand;
        }
    }

    public void HandleThrowingInput()
    {
        target = Input.mousePosition;

        if (Input.GetMouseButton(0) && throwingState == ThrowingState.PackageInHand)
        {
            throwingState = ThrowingState.Aiming;
        }
        else if (Input.GetMouseButtonUp(0) && throwingState == ThrowingState.Aiming)
        {
            throwingState = ThrowingState.Throw;
        }
    }

    void Throw()
    {
        GameObject packageObj = ProjectilePool.Instance.GetProjectileFromPool(nextPackage.tag);
        PackageScript package = packageObj.GetComponent<PackageScript>();

        package.StartPackage(transform.position, Vector3.zero);
        Vector3 throwDirection = Input.mousePosition;
        throwDirection = Camera.main.ScreenToWorldPoint(throwDirection);
        throwDirection = throwDirection - transform.position;
        throwDirection.z = 0f;
        throwDirection.Normalize();

        package.SetStartForce(new Vector2(throwDirection.x * currentThrowForce, throwDirection.y * currentThrowForce));
        currentThrowForce = startThrowForce;

        _throwSound.Play();
    }
}
