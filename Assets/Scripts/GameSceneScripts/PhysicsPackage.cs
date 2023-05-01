using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPackage : MonoBehaviour, Punchable
{
    public float _punchForce;
    public Rigidbody2D _rb;

    public bool _wasDispensed;
    public Vector3 dispensedVector;

    public void Punched(Vector3 dir)
    {
        _wasDispensed = false;
        _rb.AddForce(dir * _punchForce, ForceMode2D.Impulse);
    }
    private void FixedUpdate()
    {
        if (_wasDispensed)
        {
            MoveContinuously();
        }
    }

    public void InitDispense(Vector3 vec)
    {
        _wasDispensed = true;
        dispensedVector = vec;
    }
    private void MoveContinuously()
    {
        _rb.velocity = dispensedVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PackageScript>()) return;
        _wasDispensed = false;
    }
    public void DestroyPackage()
    {
        ProjectilePool.Instance.AddToPool(gameObject);
    }
    public void IncineratePackage()
    {
        ProjectilePool.Instance.AddToPool(gameObject);
        ProjectilePool.Instance.GetProjectileFromPool("IncineratorSound");
    }
}
