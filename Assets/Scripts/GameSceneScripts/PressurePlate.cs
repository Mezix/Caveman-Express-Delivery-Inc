using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool _activated;
    public SpriteRenderer sr;

    public List<GameObject> _objectsNearby = new List<GameObject>();

    public AudioSource _pressurePlateSoundOn;
    public AudioSource _pressurePlateSoundOff;
    private void Awake()
    {
        Activate(false);
    }
    public void Activate(bool activated, bool playSound = false)
    {
        if (_activated == activated) return;
        if (activated)
        {
            sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Tileset_pressureplate"), "tileset_pressureplate_1");
            if(playSound) _pressurePlateSoundOn.Play();
            _pressurePlateSoundOff.Stop();
        }
        else
        {
            sr.sprite = HM.GetSpriteFromSpritesheet(GS.Props("Tileset_pressureplate"), "tileset_pressureplate_0");
            if (playSound) _pressurePlateSoundOff.Play();
            _pressurePlateSoundOn.Stop();
        }
        _activated = activated;
    }

    private void FixedUpdate()
    {
        if (_objectsNearby.Count > 0)
        {
            Activate(true, true);
        }
        else
        {
            Activate(false, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PhysicsPackage pp))
        {
            _objectsNearby.Add(pp.gameObject);
        }
        if (collision.TryGetComponent(out PlayerController pCon))
        {
            _objectsNearby.Add(pCon.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PhysicsPackage pp))
        {
            _objectsNearby.Remove(pp.gameObject);
        }
        if (collision.TryGetComponent(out PlayerController pCon))
        {
            _objectsNearby.Remove(pCon.gameObject);
        }
    }
}