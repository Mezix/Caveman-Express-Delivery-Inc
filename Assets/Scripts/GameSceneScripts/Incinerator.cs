using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incinerator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PhysicsPackage pp))
        {
            pp.DestroyPackage();
        }
        if (collision.TryGetComponent(out PackageScript p))
        {
            p.DestroyPackage();
        }
        if (collision.TryGetComponent(out PlayerController pCon))
        {
            pCon.Perish();
        }
    }
}
