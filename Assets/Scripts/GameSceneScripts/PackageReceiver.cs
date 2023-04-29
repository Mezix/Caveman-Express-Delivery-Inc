using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PackageReceiver : MonoBehaviour
{
    public bool alreadyActivated;

    private void Start()
    {
        alreadyActivated = false;
    }

    public void GivePoints()
    {
        alreadyActivated = true;
        REF.pUI.AddPoints(100);
    }
}
