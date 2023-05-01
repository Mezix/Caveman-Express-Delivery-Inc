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
        REF.pUI.AddPoints(pointsToGive);
    }

    private void UpdateReceiverDirection()
    {
        switch (ReceiverDir)
        {
            case ReceiverDirection.Left:
                //transform.localPosition = new Vector2(-0.5f, 0);
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 90));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_4");
                break;
            case ReceiverDirection.Up:
                //transform.localPosition = new Vector2(0, 0.5f);
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 0));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_2");
                break;
            case ReceiverDirection.Down:
                //transform.localPosition = new Vector2(0, -0.5f);
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 180));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_1");
                break;
            case ReceiverDirection.Right:
                //transform.localPosition = new Vector2(0.5f, 0);
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 270));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_3");
                break;
            default: //default = Hole
                //transform.localPosition = new Vector2(0, 0);
                HM.RotateLocalTransformToAngle(transform, new Vector3(0, 0, 0));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Package_Receivers"), "Package_Receivers_0");
                break;
        }
    }

}
