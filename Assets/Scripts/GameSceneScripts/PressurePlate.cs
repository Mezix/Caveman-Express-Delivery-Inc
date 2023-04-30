using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool _activated;
    public SpriteRenderer sr;
    public Color _lightColor;
    public Color _darkColor;

    public List<GameObject> _objectsNearby = new List<GameObject>();
    public void Activate(bool activated)
    {
        _activated = activated;

        if(activated)
        {
            sr.color = _darkColor;
        }
        else
        {
            sr.color = _lightColor;
        }
    }

    private void FixedUpdate()
    {
        if(_objectsNearby.Count > 0)
        {
            Activate(true);
        }
        else
        {
            Activate(false);
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