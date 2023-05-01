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

        foreach(PhysicsPackage pp in physicsPackagesToDestroy)
        {
            pp.DestroyPackage();
        }
        foreach (PackageScript p in packagesToDestroy)
        {
            p.IncineratePackage();
        }
        if (player) player.Perish();
        physicsPackagesToDestroy.Clear();
        packagesToDestroy.Clear();
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
