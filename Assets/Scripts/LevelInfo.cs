using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public float _timeInSecondsForThisLevel;
    public bool shouldStartTimer;
    public Loader.Scene _thisScene;

    public bool shouldResetScoreAndTime;
    private void Start()
    {
        Loader._currentScene = _thisScene;

        if (!REF.score) return;
        REF.score.SetCurrentTime(this);
        REF.score.StartTimer(shouldStartTimer);
    }
}
