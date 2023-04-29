using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButtonScript : MonoBehaviour
{
    public SpriteRenderer buttonRenderer;
    public DoorScript doorScript;
    public List<PackageReceiver> holeList;
    private bool readyToHit;


    private void Start()
    {

        if (holeList.Count > 0)
        {
            buttonRenderer.color = Color.red;
            readyToHit = false;
        } else
        {
            buttonRenderer.color = Color.green;
            readyToHit = true;
        }

    }
    void Update()
    {

        List<PackageReceiver> alreadyActivatedHoleList = new List<PackageReceiver>();
        int alreadyActivatedHoles = 0;
        foreach (PackageReceiver hole in holeList)
        {
            if (hole.alreadyActivated)
            {
                alreadyActivatedHoles++;
                alreadyActivatedHoleList.Add(hole);
            }
        }
        if (holeList.Count <= alreadyActivatedHoles)
        {
            readyToHit = true;
            buttonRenderer.color = Color.green;
        }
        foreach(PackageReceiver alrActHol in alreadyActivatedHoleList)
        {
            holeList.Remove(alrActHol);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PackageScript p) && readyToHit)
        {
            buttonRenderer.color = Color.gray;
            doorScript.OpenDoor();
        }
    }
}
