using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public float _timeInSecondsForThisLevel;
    public bool shouldStartTimer;
    public Loader.Scene _thisScene;
    public ConversationScriptObj convoOnLoad;

    public bool shouldResetScoreAndTime;
    private void Start()
    {
        StartCoroutine(playDialogue());
        Loader._currentScene = _thisScene;
        if (!REF.score) return;
        REF.score.SetCurrentTime(this);
        REF.score.StartTimer(shouldStartTimer);
    }

    private IEnumerator playDialogue()
    {
        for (int i = 0; i <= 50; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        if (REF.dialog) REF.dialog.StartDialogue(convoOnLoad);
    }
}
