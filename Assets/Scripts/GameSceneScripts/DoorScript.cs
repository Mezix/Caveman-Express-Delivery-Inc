using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public BoxCollider2D upperDoorCollider;
    public BoxCollider2D lowerDoorCollider;

    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;

    public void OpenDoor(bool openedState)
    {
        //upperDoorCollider.isTrigger = true;
        //lowerDoorCollider.isTrigger = true;
        if (openedState)
        {
            doorOpenSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            doorOpenSound.Play();
        }
        else
        {
            doorCloseSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            doorCloseSound.Play();
        }
        upperDoorCollider.gameObject.SetActive(!openedState);
        lowerDoorCollider.gameObject.SetActive(!openedState);
    }
}
