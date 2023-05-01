using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public float _timeInSecondsForThisLevel;
    public bool shouldStartTimer;

    public bool shouldResetScoreAndTime;
    private void Start()
    {
        if (!REF.score) return;
        REF.score.SetCurrentTime(this);
        REF.score.StartTimer(shouldStartTimer);
    }
}
