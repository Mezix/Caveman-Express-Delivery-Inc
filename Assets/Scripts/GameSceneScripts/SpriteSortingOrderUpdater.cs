using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortingOrderUpdater : MonoBehaviour
{
    private SpriteRenderer sr;
    public int offset;
    public bool floorOrCeilToInt; // true => floor, false = Ceil
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        if(floorOrCeilToInt) sr.sortingOrder = Mathf.FloorToInt(-transform.position.y + offset);
        else sr.sortingOrder = Mathf.CeilToInt(-transform.position.y + offset);
    }
}
