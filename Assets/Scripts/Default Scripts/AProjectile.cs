using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{
    public float CurrentLifeTime { get; protected set; }
    public float MaxLifetime { get; set; }
    public int Damage { get; set; }
    public float ProjectileSpeed { get; set; }
    public bool HitPlayer { get; set; }

    public bool HasDoneDamage { get; set; }

    //  Misc

    [HideInInspector] public Rigidbody _projectileRB;
    [HideInInspector] public Collider _projectileCollider;
    [HideInInspector] public bool _despawnAnimationPlaying;

    public virtual void Awake()
    {
        _projectileRB = GetComponentInChildren<Rigidbody>();
        _projectileCollider = GetComponentInChildren<Collider>();
        MaxLifetime = 3;
    }
    public virtual void Start()
    {

    }
    public virtual void FixedUpdate()
    {
        if (!_despawnAnimationPlaying) MoveProjectile();
    }
    public virtual void OnEnable()
    {
        CurrentLifeTime = 0;
        _despawnAnimationPlaying = false;
        HasDoneDamage = false;
    }
    public virtual void Update()
    {
        CurrentLifeTime += Time.deltaTime;
        CheckLifetime();
    }
    public virtual void MoveProjectile()
    {
        _projectileRB.MovePosition(transform.position + transform.forward * ProjectileSpeed * Time.deltaTime);
    }
    protected void CheckLifetime() //a function that checks if our projectile has reached the end of its lifespan, and then decides what to do now
    {
        if (CurrentLifeTime >= MaxLifetime && !_despawnAnimationPlaying)
        {
            StartCoroutine(DespawnAnimation());
        }
    }
    protected void DespawnBullet()
    {
        ProjectilePool.Instance.AddToPool(gameObject);
    }
    public virtual void SetBulletStatsAndTransformToWeaponStats(Transform t)
    {
        HitPlayer = false;
        transform.position = t.transform.position;
        transform.rotation = t.transform.rotation;
    }
    public virtual IEnumerator DespawnAnimation()
    {
        _despawnAnimationPlaying = true;
        yield return new WaitForSeconds(0f);
        DespawnBullet();
    }
    public virtual void OnTriggerEnter(Collider col)
    {
        if (!HasDoneDamage)
        {
            if (HitPlayer)
            {

            }
            else
            {

            }
        }
    }
}
