using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PackageReceiver : MonoBehaviour
{
    public bool alreadyActivated;
    public int pointsToGive;

    private void Start()
    {
        alreadyActivated = false;
        pointsToGive = 100;
    }

    public void GivePoints()
    {
        alreadyActivated = true;
        Debug.Log(this);
        REF.pUI.AddPoints(pointsToGive);
    }
}
