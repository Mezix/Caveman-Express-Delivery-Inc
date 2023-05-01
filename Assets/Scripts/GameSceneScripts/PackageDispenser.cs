using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageDispenser : MonoBehaviour
{
    public GameObject _packagePrefab;
    public SpriteRenderer sr;
    public Transform _dispenseTransform;
    public float _dispenseForce;

    public float timeSinceLastDispensation;
    public float dispenserCooldown;

    public List<PhysicsPackage> _packagesCreated = new List<PhysicsPackage>();
    public DispenserDirection dispenserDir;
    public enum DispenserDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    private void Awake()
    {
        _dispenseForce = 5;
        dispenserCooldown = 2;
    }
    private void Start()
    {
        UpdateDispenserDirection();
    }

    private void UpdateDispenserDirection()
    {
        switch (dispenserDir)
        {
            case DispenserDirection.Left:
                _dispenseTransform.localPosition = new Vector2(-0.5f, 0);
                HM.RotateLocalTransformToAngle(_dispenseTransform, new Vector3(0,0,180));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Dispenser_SpriteSheet"), "Dispenser_SpriteSheet_2");
                break;
            case DispenserDirection.Up:
                _dispenseTransform.localPosition = new Vector2(0, 0.5f);
                HM.RotateLocalTransformToAngle(_dispenseTransform, new Vector3(0, 0, 90));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Dispenser_SpriteSheet"), "Dispenser_SpriteSheet_3");
                break;
            case DispenserDirection.Down:
                _dispenseTransform.localPosition = new Vector2(0, -0.5f);
                HM.RotateLocalTransformToAngle(_dispenseTransform, new Vector3(0, 0, 270));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Dispenser_SpriteSheet"), "Dispenser_SpriteSheet_0");
                break;
            default: //default = Right 
                _dispenseTransform.localPosition = new Vector2(0.5f, 0);
                HM.RotateLocalTransformToAngle(_dispenseTransform, new Vector3(0, 0, 0));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Dispenser_SpriteSheet"), "Dispenser_SpriteSheet_1");
                break;
        }
    }

    private void Update()
    {
        timeSinceLastDispensation += Time.deltaTime;
        CheckDispense();
    }

    private void CheckDispense()
    {
        if (timeSinceLastDispensation > dispenserCooldown)
        {
            DispensePackage();
        }
    }

    public void DispensePackage()
    {
        timeSinceLastDispensation = 0;
        GameObject g = ProjectilePool.Instance.GetProjectileFromPool(_packagePrefab.tag);
        g.transform.position = _dispenseTransform.position;

        if (g.TryGetComponent(out PhysicsPackage p))
        {
            p.InitDispense(_dispenseTransform.right * _dispenseForce);
        }
    }
}