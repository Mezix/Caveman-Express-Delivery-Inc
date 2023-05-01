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

    //  Disappearing
    public float timeSinceNoVelocity;
    public float timeUntilDissapear;
    public bool disapearing;

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
        if (timeSinceNoVelocity > timeUntilDissapear)
        {
            disapearing = true;
            packageCollider.isTrigger = true;
        }
        if (disapearing)
        {
            DisappearOverTime();
        }
        if (transform.localScale.x < 0)
        {
            PackageWasReceived();
            return;
        }
        if (packageSprite.color.a <= 0)
        {
            PackageDespawned();
            return;
        }
        if (packageRB.velocity.magnitude <= 0.1f)
        {
            packageRB.velocity = Vector3.zero;

            if (packageReceiver)
            {
                if (!pointsGiven)
                {
                    packageCollider.isTrigger = true;
                    timeSinceNoVelocity = 0;
                    pointsGiven = true;
                    packageReceiver.GivePoints();
                }
                MoveTowardsPackageReceiverAndShrink();
            }
            else timeSinceNoVelocity += Time.deltaTime;
        }
        else timeSinceNoVelocity = 0;
    }

    private void MoveTowardsPackageReceiverAndShrink()
    {
        transform.position = Vector3.Lerp(transform.position, packageReceiver.transform.position, 0.5f * Time.deltaTime);
        transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f, transform.localScale.z);
    }

    private void DisappearOverTime()
    {
        packageSprite.color = new Color(packageSprite.color.r, packageSprite.color.g, packageSprite.color.b, packageSprite.color.a - 0.04f);
    }

    public void StartPackage(Vector3 transformPosition, Vector3 rotation)
    {
        timeSinceNoVelocity = 0;
        transform.SetParent(null);
        transform.localScale = Vector3.one;
        transform.position = transformPosition;
        HM.RotateLocalTransformToAngle(transform, rotation);
        packageSprite.color = new Color(packageSprite.color.r, packageSprite.color.g, packageSprite.color.b, 1);
        packageReceiver = null;
        packageCollider.isTrigger = false;
        pointsGiven = false;
        disapearing = false;
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
                PackageWasReceived();
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
    public void PackageDespawned()
    {
        ProjectilePool.Instance.AddToPool(gameObject);
    }

    public void IncineratePackage()
    {
        ProjectilePool.Instance.AddToPool(gameObject);
        ProjectilePool.Instance.GetProjectileFromPool("IncineratorSound");
    }

    public void PackageWasReceived()
    {
        ProjectilePool.Instance.AddToPool(gameObject);
    }

}
