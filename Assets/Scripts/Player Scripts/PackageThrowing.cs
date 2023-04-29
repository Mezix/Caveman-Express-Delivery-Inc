using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PackageThrowing : MonoBehaviour
{
    private float loadingSpeed;
    public float currentThrowForce;
    private float maxThrowingSpeed;
    private Vector3 target;
    private ThrowingState throwingState;

    void Start()
    {
        loadingSpeed = 0.4f;
        currentThrowForce = 5f;
        maxThrowingSpeed = 50f;
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
        //PackageScript package = Instantiate(Resources.Load(GS.Prefabs("Package"), typeof(PackageScript)) as PackageScript, transform.position, new Quaternion(0,0,0,0));
        GameObject packageObj = ProjectilePool.Instance.GetProjectileFromPool("Package1x1");
        PackageScript package = packageObj.GetComponent<PackageScript>();
        Vector3 throwDirection = Input.mousePosition;
        throwDirection = Camera.main.ScreenToWorldPoint(throwDirection);
        throwDirection = throwDirection - transform.position;
        throwDirection.z = 0f;
        throwDirection.Normalize();
        package.SetStartForce(new Vector2(throwDirection.x * currentThrowForce, throwDirection.y * currentThrowForce));
        currentThrowForce = 5f;
    }

    private enum ThrowingState
    {
        PackageInHand,
        Aiming,
        Throw
    }
}
