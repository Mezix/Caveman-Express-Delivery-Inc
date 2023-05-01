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
    public float _dispenseStartDistance;

    public float timeSinceLastDispensation;
    public float dispenserCooldown;

    public float forceOffset;
    public float cooldownOffset;

    public List<PhysicsPackage> _packagesCreated = new List<PhysicsPackage>();
    public DispenserDirection dispenserDir;
    public AudioSource _dispenseSound;
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
        timeSinceLastDispensation = 0;
        dispenserCooldown = 2;
        _dispenseStartDistance = 1f;
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
                _dispenseTransform.localPosition = new Vector2(-_dispenseStartDistance, 0);
                HM.RotateLocalTransformToAngle(_dispenseTransform, new Vector3(0,0,180));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Dispenser_SpriteSheet"), "Dispenser_SpriteSheet_2");
                break;
            case DispenserDirection.Up:
                _dispenseTransform.localPosition = new Vector2(0, _dispenseStartDistance);
                HM.RotateLocalTransformToAngle(_dispenseTransform, new Vector3(0, 0, 90));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Dispenser_SpriteSheet"), "Dispenser_SpriteSheet_3");
                break;
            case DispenserDirection.Down:
                _dispenseTransform.localPosition = new Vector2(0, -_dispenseStartDistance/2);
                HM.RotateLocalTransformToAngle(_dispenseTransform, new Vector3(0, 0, 270));
                sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Dispenser_SpriteSheet"), "Dispenser_SpriteSheet_0");
                break;
            default: //default = Right 
                _dispenseTransform.localPosition = new Vector2(_dispenseStartDistance, 0);
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
        if (timeSinceLastDispensation > (dispenserCooldown + cooldownOffset))
        {
            DispensePackage();
        }
    }

    public void DispensePackage()
    {
        _dispenseSound.pitch = HM.GetFloatWithRandomVariance(_dispenseSound.pitch, 0.2f);
        _dispenseSound.Play();
        timeSinceLastDispensation = 0;
        GameObject g = ProjectilePool.Instance.GetProjectileFromPool(_packagePrefab.tag);
        g.transform.position = _dispenseTransform.position;

        if (g.TryGetComponent(out PhysicsPackage p))
        {
            p.InitDispense(_dispenseTransform.right * (_dispenseForce + forceOffset));
        }
    }
}
