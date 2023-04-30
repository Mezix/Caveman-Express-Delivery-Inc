using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public BoxCollider2D upperDoorCollider;
    public BoxCollider2D lowerDoorCollider;
    public void OpenDoor(bool openedState)
    {
        //upperDoorCollider.isTrigger = true;
        //lowerDoorCollider.isTrigger = true;
        upperDoorCollider.gameObject.SetActive(!openedState);
        lowerDoorCollider.gameObject.SetActive(!openedState);
    }
}
