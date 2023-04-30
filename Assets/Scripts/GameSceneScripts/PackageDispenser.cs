using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageDispenser : MonoBehaviour
{
    public GameObject _packagePrefab;
    public Transform _dispenseTransform;
    public float _dispenseForce;

    public float timeSinceLastDispensation;
    public float dispenserCooldown;

    public List<PhysicsPackage> _packagesCreated = new List<PhysicsPackage>();
    private void Awake()
    {
        _dispenseForce = 5;
        dispenserCooldown = 2;
    }
    private void Update()
    {
        timeSinceLastDispensation += Time.deltaTime;
        CheckDispense();
    }

    private void CheckDispense()
    {
        if (timeSinceLastDispensation > dispenserCooldown)
        {
            DispensePackage();
        }
    }

    public void DispensePackage()
    {
        timeSinceLastDispensation = 0;
        GameObject g = ProjectilePool.Instance.GetProjectileFromPool(_packagePrefab.tag);
        g.transform.position = _dispenseTransform.position;

        if (g.TryGetComponent(out PhysicsPackage p))
        {
            p.InitDispense(_dispenseTransform.right * _dispenseForce);
        }
    }
}
