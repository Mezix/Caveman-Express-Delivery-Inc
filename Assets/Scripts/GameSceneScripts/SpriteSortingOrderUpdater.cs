using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortingOrderUpdater : MonoBehaviour
{
    private SpriteRenderer sr;
    public int offset;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        sr.sortingOrder = Mathf.CeilToInt(-transform.position.y + offset);
    }
}
