using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMan : MonoBehaviour
{

    public int bossManHealth;
    public DoorScript doorScript;
    public List<PackageDispenser> _packageDispenserMinions = new List<PackageDispenser>();

    private void Start()
    {
        bossManHealth = 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PackageScript p))
        {
            bossManHealth--;
        }
        if (bossManHealth <= 0)
        {
            gameObject.SetActive(false);
            doorScript.OpenDoor(true);
            foreach (PackageDispenser minion in _packageDispenserMinions)
            {
                minion.dispenserCooldown = 999999;
            }
        }
    }
}
