using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageScript : MonoBehaviour, Punchable
{
    public Rigidbody2D packageRB;
    public BoxCollider2D packageCollider;
    private Vector2 velocity;
    public float _punchForce;
    public AudioSource wallHitSound;
    public SpriteRenderer packageSprite;

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
            packageCollider.isTrigger = true;
            packageRB.velocity = Vector3.zero;
            if (packageReceiver)
            {
                if (!pointsGiven)
                {
                    pointsGiven = true;
                    packageReceiver.GivePoints();
                }

                transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f, transform.localScale.z);
            }
            else
            {
                packageSprite.color = new Color(packageSprite.color.r, packageSprite.color.g, packageSprite.color.b, packageSprite.color.a - 0.04f);
            }

        }
        if (transform.localScale.x < 0)
        {
            ReceivePackage();
        }
        else if (packageSprite.color.a <= 0)
        {
            DestroyPackage();
        }
    }

    public void StartPackage(Vector3 transformPosition, Vector3 rotation)
    {
        transform.SetParent(null);
        transform.localScale = Vector3.one;
        transform.position = transformPosition;
        HM.RotateLocalTransformToAngle(transform, rotation);
        packageSprite.color = new Color(packageSprite.color.r, packageSprite.color.g, packageSprite.color.b, 1);
        packageReceiver = null;
        packageCollider.isTrigger = false;
        pointsGiven = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PackageReceiver p))
        {
            packageReceiver = p;
            if (packageReceiver.wallReceiver)
            {
                pointsGiven = true;
                packageReceiver.GivePoints();
                ReceivePackage();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PackageReceiver p))
        {
            packageReceiver = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        wallHitSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        wallHitSound.Play();
    }
    public void Punched(Vector3 dir)
    {
        if (packageCollider.isTrigger) return;
        packageRB.AddForce(dir * _punchForce, ForceMode2D.Impulse);
    }
    public void DestroyPackage()
    {
        ProjectilePool.Instance.AddToPool(gameObject);
    }

    public void IncineratePackage()
    {
        ProjectilePool.Instance.AddToPool(gameObject);
    }

    public void ReceivePackage()
    {
        ProjectilePool.Instance.AddToPool(gameObject);
    }

}
