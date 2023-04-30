using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPackage : MonoBehaviour, Punchable
{
    public float _punchForce;
    public Rigidbody2D _rb;
    public void Punched(Vector3 dir)
    {
        _rb.AddForce(dir * _punchForce, ForceMode2D.Impulse);
    }
}
