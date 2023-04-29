using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageThrowing : MonoBehaviour
{

    private float loadingSpeed;
    private float currentThrowForce;
    private float maxThrowingSpeed;
    private Vector3 target;
    private ThrowingState throwingState;
    void Start()
    {
        loadingSpeed = 0.1f;
        currentThrowForce = 0f;
        maxThrowingSpeed = 5f;
        throwingState = ThrowingState.PackageInHand;
    }

    void Update()
    {
        ThrowingInput();
    }

    void FixedUpdate()
    {
        if(throwingState == ThrowingState.Aiming)
        {
            currentThrowForce = Mathf.Min(maxThrowingSpeed, currentThrowForce + loadingSpeed);
        }
        else if(throwingState == ThrowingState.Throw)
        {
            Throw();
            throwingState = ThrowingState.PackageInHand;
        }
    }

    void ThrowingInput()
    {
        target = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            throwingState = ThrowingState.Aiming;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            throwingState = ThrowingState.Throw;
        }
    }

    void Throw()
    {
        PackageScript package = Instantiate(Resources.Load(GS.Prefabs("Package"), typeof(PackageScript)) as PackageScript);
        package.SetStartForce(new Vector2(10, 10));
    }

    private enum ThrowingState
    {
        PackageInHand,
        Aiming,
        Throw
    }
}
