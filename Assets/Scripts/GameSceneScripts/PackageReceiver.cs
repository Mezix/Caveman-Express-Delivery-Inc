using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PackageReceiver : MonoBehaviour
{
    public bool alreadyActivated;
    public int pointsToGive;

    public bool wallReceiver;
    public SpriteRenderer sr;

    public AudioSource _packageInGoal;

    public ReceiverDirection ReceiverDir;
    public enum ReceiverDirection
    {
        Left,
        Right,
        Up,
        Down,
        Hole
    }

    private void Start()
    {
        alreadyActivated = false;
        pointsToGive = 100;
        UpdateReceiverDirection();
    }

    public void GivePoints()
    {
        alreadyActivated = true;
        _packageInGoal.Play();
        if (REF.score) REF.score.AddPoints(pointsToGive);
    }

    private void UpdateReceiverDirection()
    {
        switch (ReceiverDir)
        {
            case ReceiverDirection.Left:
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 90));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_4");
                break;
            case ReceiverDirection.Up:
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 0));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_2");
                break;
            case ReceiverDirection.Down:
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 180));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_1");
                break;
            case ReceiverDirection.Right:
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 270));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_3");
                break;
            default: //default = Hole
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 0));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_0");
                break;
        }
    }

}
