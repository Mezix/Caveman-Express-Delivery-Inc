using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncineratorSound : MonoBehaviour
{
    public AudioSource incineratorSound;
    private void OnEnable()
    {
        incineratorSound.pitch = HM.GetFloatWithRandomVariance(incineratorSound.pitch, 0.2f);  
        incineratorSound.Play();
        StartCoroutine(AddToPool());
    }

    private IEnumerator AddToPool()
    {
        yield return new WaitForSeconds(1f);
        ProjectilePool.Instance.AddToPool(gameObject);
    }
}
