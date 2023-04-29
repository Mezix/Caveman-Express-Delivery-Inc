using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject _cursor;
    public GameObject _throwBar;
    public Image _throwFill;

    private void Update()
    {

        UpdateCursorBar();
    }

    private void UpdateCursorBar()
    {
        _cursor.transform.position = Input.mousePosition;
        if (REF.pCon._throw.throwingState == PackageThrowing.ThrowingState.Aiming) _throwBar.gameObject.SetActive(true);
        else _throwBar.gameObject.SetActive(false);

        _throwFill.fillAmount = REF.pCon._throw.currentThrowForce/ REF.pCon._throw.maxThrowingSpeed;
    }
}
