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

    // Score

    public Text _scoreText;
    public int _points;

    //  Cursor Stuff
    public GameObject _cursor;
    public GameObject _throwBar;
    public Image _throwFill;

    //  Game Over
    public GameObject _gameOverScreen;
    public ButtonScript _retryButton;
    public ButtonScript _quitGameButton;

    private void Awake()
    {
        REF.pUI = this;
    }
    private void Update()
    {
        UpdateCursorBar();
        UpdateTimerText();
        UpdatePoints();
    }

    private void Start()
    {
        Time.timeScale = 1;
        InitButtons();
        _gameOverScreen.SetActive(false);
    }

    private void InitButtons()
    {
        _retryButton._button.onClick.AddListener(() => RetryCurrentScene()); ;
        _quitGameButton._button.onClick.AddListener(() => Application.Quit());
    }

    private void RetryCurrentScene()
    {
        Loader.Load(Loader._currentScene);
    }

    // Timer
    public void UpdateTimerText()
    {
        if (!REF.score) return;
        TimeSpan timespan = TimeSpan.FromSeconds(REF.score._timeLeftInSeconds);
        _timerText.text = string.Format("{0:D2}:{1:D2}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
    }

    // Score Stuff

    private void UpdatePoints()
    {
        if (REF.score) SetScoreText(REF.score._currentScore);
    }

    public void SetScoreText(int points)
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
        REF.pCon._lockAllActions = true;
        REF.pUI._gameOverScreen.SetActive(true);
    }
}
