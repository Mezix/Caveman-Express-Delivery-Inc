using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageScript : MonoBehaviour, Punchable
{
    public Rigidbody2D packageRB;
    public BoxCollider2D packageCollider;
    private Vector2 velocity;
    public float _punchForce;

    public PackageReceiver packageReceiver;
    public bool pointsGiven;

    public void SetStartForce(Vector2 startVelocity)
    {
        velocity = startVelocity;
        packageRB.AddForce(new Vector3(velocity.x, velocity.y, 0), ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        LifetimeBehavior();
    }

    private void LifetimeBehavior()
    {
        if (packageRB.velocity.magnitude <= 0.1f)
        {
            if (packageReceiver && !pointsGiven)
            {
                pointsGiven = true;
                packageReceiver.GivePoints();
                packageRB.velocity = Vector3.zero;
            }
            packageCollider.isTrigger = true;
            transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f, transform.localScale.z);
        }
        if (transform.localScale.x < 0)
        {
            ProjectilePool.Instance.AddToPool(gameObject);
        }
    }

    public void StartPackage(Vector3 transformPosition, Vector3 rotation)
    {
        transform.SetParent(null);
        transform.localScale = Vector3.one;
        transform.position = transformPosition;
        HM.RotateLocalTransformToAngle(transform, rotation);
        packageCollider.isTrigger = false;
        pointsGiven = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PackageReceiver p))
        {
            packageReceiver = p;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PackageReceiver p))
        {
            packageReceiver = null;
        }
    }

    public void Punched(Vector3 dir)
    {
        if (packageCollider.isTrigger) return;
        packageRB.AddForce(dir * _punchForce, ForceMode2D.Impulse);
    }
}
