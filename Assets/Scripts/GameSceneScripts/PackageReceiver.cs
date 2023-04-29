using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PackageReceiver : MonoBehaviour
{
    public void GivePoints()
    {
        REF.pUI.AddPoints(100);
    }
}
