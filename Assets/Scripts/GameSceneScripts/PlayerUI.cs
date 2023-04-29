using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    //  Main Menu Stuff


    //  Timer

    // Score

    public Text _scoreText;
    public int _points;

    //  Cursor Stuff
    public GameObject _cursor;
    public GameObject _throwBar;
    public Image _throwFill;
    private void Awake()
    {
        REF.pUI = this;
    }
    private void Update()
    {

        UpdateCursorBar();
    }
    private void Start()
    {
        ResetScore();
    }

    // Score Stuff

    public void ResetScore()
    {
        _points = 0;
        SetScore(_points);
    }

    public void AddPoints(int points)
    {
        _points += points;
        SetScore(_points);
    }

    private void SetScore(int points)
    {
        _scoreText.text = points.ToString();
    }

    //  Cursor Stuff
    private void UpdateCursorBar()
    {
        _cursor.transform.position = Input.mousePosition;
        if (REF.pCon._throw.throwingState == PackageThrowing.ThrowingState.Aiming) _throwBar.gameObject.SetActive(true);
        else _throwBar.gameObject.SetActive(false);

        _throwFill.fillAmount = REF.pCon._throw.currentThrowForce/ REF.pCon._throw.maxThrowingSpeed;
    }
}
