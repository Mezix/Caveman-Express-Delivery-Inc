using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageThrowing : MonoBehaviour
{
    public enum ThrowingState
    {
        Cooldown,
        PackageInHand,
        Aiming,
        Throw
    }

    private float loadingSpeed;
    private float startThrowForce;
    private float currentThrowForce;
    private float maxThrowingSpeed;
    private float throwCooldown;
    private float currentThrowCooldown;
    public ThrowingState throwingState;

    void Start()
    {
        startThrowForce = 5f;
        loadingSpeed = 0.4f;
        maxThrowingSpeed = 50f;
        currentThrowCooldown = 0f;
        throwCooldown = 50f;
        currentThrowForce = startThrowForce;
        throwingState = ThrowingState.PackageInHand;
    }

    void Update()
    {
        ThrowingInput();
    }

    void FixedUpdate()
    {
        if (throwingState == ThrowingState.Aiming)
        {
            currentThrowForce = Mathf.Min(maxThrowingSpeed, currentThrowForce + loadingSpeed);
        }
        else if (throwingState == ThrowingState.Throw)
        {
            Throw();
            throwingState = ThrowingState.Cooldown;
            currentThrowCooldown = throwCooldown;
        }
        else if (throwingState == ThrowingState.Cooldown)
        {
            currentThrowCooldown--;
            if(currentThrowCooldown <= 0)
            {
                throwingState = ThrowingState.PackageInHand;
            }
        }
    }

    void ThrowingInput()
    {
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
        PackageScript package = Instantiate(Resources.Load(GS.Prefabs("Package"), typeof(PackageScript)) as PackageScript, transform.position, new Quaternion(0, 0, 0, 0));
        Vector3 throwDirection = Input.mousePosition;
        throwDirection = Camera.main.ScreenToWorldPoint(throwDirection);
        throwDirection = throwDirection - transform.position;
        throwDirection.z = 0f;
        throwDirection.Normalize();
        package.SetStartForce(new Vector2(throwDirection.x * currentThrowForce, throwDirection.y * currentThrowForce));
        currentThrowForce = startThrowForce;
    }


}
