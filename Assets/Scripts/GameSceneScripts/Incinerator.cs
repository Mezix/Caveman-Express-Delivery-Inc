using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incinerator : MonoBehaviour
{
    public bool _activated;
    public Animator anim;

    [HideInInspector] public List<PhysicsPackage> physicsPackagesToDestroy = new List<PhysicsPackage>();
    [HideInInspector] public List<PackageScript> packagesToDestroy = new List<PackageScript>();
    [HideInInspector] public PlayerController player = null;

    private void Update()
    {
        DestroyObjects();
    }
    private void Start()
    {
        ActivateIncinerator(_activated);
    }
    public void ActivateIncinerator(bool activated)
    {
        _activated = activated;
        if (activated)
        {
            anim.SetTrigger("Open");
            anim.ResetTrigger("Close");
        }
        else
        {
            anim.SetTrigger("Close");
            anim.ResetTrigger("Open");
        }

    }
    private void DestroyObjects()
    {
        if (!_activated) return;
        List<PhysicsPackage> tmpPhysicsPackagesToDestroy = new List<PhysicsPackage>();
        List<PackageScript> tmpPackagesToDestroy = new List<PackageScript>();

        //  Deep Copy

        foreach (PhysicsPackage pp in physicsPackagesToDestroy)
        {
            tmpPhysicsPackagesToDestroy.Add(pp);
        }
        foreach (PackageScript p in packagesToDestroy)
        {
            tmpPackagesToDestroy.Add(p);
        }

        //  Removal
        foreach (PhysicsPackage pp in tmpPhysicsPackagesToDestroy)
        {
            if (physicsPackagesToDestroy.Contains(pp)) physicsPackagesToDestroy.Remove(pp);
            pp.IncineratePackage();
        }
        foreach (PackageScript p in tmpPackagesToDestroy)
        {
            if (packagesToDestroy.Contains(p)) packagesToDestroy.Remove(p);
            p.IncineratePackage();
        }

        if (player) player.Incinerate();
        player = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PhysicsPackage pp))
        {
            physicsPackagesToDestroy.Add(pp);
        }
        if (collision.TryGetComponent(out PackageScript p))
        {
            packagesToDestroy.Add(p);
        }
        if (collision.TryGetComponent(out PlayerController pCon))
        {
            player = pCon;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PhysicsPackage pp))
        {
            physicsPackagesToDestroy.Remove(pp);
        }
        if (collision.TryGetComponent(out PackageScript p))
        {
            packagesToDestroy.Remove(p);
        }
        if (collision.TryGetComponent(out PlayerController pCon))
        {
            player = null;
        }
    }
}
