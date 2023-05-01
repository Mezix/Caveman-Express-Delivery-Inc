using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int _currentScore { get; private set; }
    public float _timeLeftInSeconds { get; private set; }

    public bool shouldCountDown = false;

    public bool _gameIsOver;

    public void StartTimer(bool start)
    {
        shouldCountDown = start;
        Debug.Log("Start Timer");
    }

    public float _timeAtStartOfLastStage;
    private void Awake()
    {
        REF.score = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (shouldCountDown) _timeLeftInSeconds -= Time.deltaTime;
        if (_timeLeftInSeconds <= 0)
        {
            if (!_gameIsOver)
            {
                _gameIsOver = true;
                REF.pUI.TriggerGameOver();
            }
        }
    }
    public void SaveTimeAtEndOfStage()
    {
        _timeAtStartOfLastStage = _timeLeftInSeconds;
    }
    public void SetCurrentTime(LevelInfo l)
    {
        _timeLeftInSeconds = _timeAtStartOfLastStage + l._timeInSecondsForThisLevel;
        Debug.Log("Level gives " + l._timeInSecondsForThisLevel + " extra seconds!");
    }
    public void ResetScore()
    {
        _currentScore = 0;
        REF.pUI.SetScoreText(_currentScore);
    }

    public void AddPoints(int points)
    {
        _currentScore += points;
        REF.pUI.SetScoreText(_currentScore);
    }
}
