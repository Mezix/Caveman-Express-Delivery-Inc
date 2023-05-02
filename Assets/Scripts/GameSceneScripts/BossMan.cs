using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMan : MonoBehaviour
{

    public int bossManHealth;
    public DoorScript doorScript;
    public List<PackageDispenser> _packageDispenserMinions = new List<PackageDispenser>();
    public SpriteRenderer bossmanSprite;

    private void Start()
    {
        bossManHealth = 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PackageScript p))
        {
            bossmanSprite.color = Color.red;
            StartCoroutine(bossManHit());
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
    private IEnumerator bossManHit()
    {
        for (int i = 0; i <= 25; i++)
        {
            bossmanSprite.color = new Color((float)i / 25, 1, 1, 1);
            yield return new WaitForFixedUpdate();
        }
    }
}
