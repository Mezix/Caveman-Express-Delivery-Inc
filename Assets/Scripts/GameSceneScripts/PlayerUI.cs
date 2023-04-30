using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //  Main Menu Stuff


    //  Timer

    public Text _timerText;
    public float _timerLeft;
    public float _levelTimer;

    // Score

    public Text _scoreText;
    public int _points;

    //  Cursor Stuff
    public GameObject _cursor;
    public GameObject _throwBar;
    public Image _throwFill;

    //  Game Over
    public GameObject _gameOverScreen;
    public bool _gameIsOver;
    public ButtonScript _retryButton;
    public ButtonScript _quitGameButton;

    private void Awake()
    {
        REF.pUI = this;
    }
    private void Update()
    {
        _timerLeft -= Time.deltaTime;
        CheckTimer();
        UpdateCursorBar();
    }

    private void Start()
    {
        Time.timeScale = 1;
        InitButtons();
        ResetScore();
        _gameOverScreen.SetActive(false);
        _gameIsOver = false;
        _levelTimer = 200;
        _timerLeft = _levelTimer;
    }

    private void InitButtons()
    {
        _retryButton._button.onClick.AddListener(() => RetryCurrentScene()); ;
        _quitGameButton._button.onClick.AddListener(() => Application.Quit());
    }

    private void RetryCurrentScene()
    {
        Loader._currentScene = Loader.Scene.LongLevel;
        Loader.Load(Loader._currentScene);
    }

    //  Timer Stuff
    private void CheckTimer()
    {
        if (_timerLeft <= 0)
        {
            _timerLeft = 0;
            if (!_gameIsOver)
            {
                UpdateTimerText();
                TriggerGameOver();
            }
        }
        else
        {
            UpdateTimerText();
        }
    }

    public void UpdateTimerText()
    {
        TimeSpan timespan = TimeSpan.FromSeconds(_timerLeft);
        _timerText.text = string.Format("{0:D2}:{1:D2}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
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

        _throwFill.fillAmount = REF.pCon._throw.currentThrowForce / REF.pCon._throw.maxThrowingSpeed;
    }

    //  Game Over

    public void TriggerGameOver()
    {
        Time.timeScale = 0;
        _gameIsOver = true;
        _gameOverScreen.SetActive(true);
        REF.pCon._lockAllActions = true;
    }
}
