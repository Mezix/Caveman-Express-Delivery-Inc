using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOpeningLevelScene : MonoBehaviour
{
    public ConversationScriptObj convo;
    private void Start()
    {
        StartCoroutine(PlayBossConvo());
    }

    private IEnumerator PlayBossConvo()
    {
        REF.pCon._lockAllActions = true;
        for(int i = 0; i < 25f; i++)
        {
            yield return new WaitForFixedUpdate();
            REF.pCon._lockAllActions = true;
        }
        REF.dialog.StartDialogue(convo);
        yield return new WaitWhile(() => REF.dialog.DialogueShown);
        REF.pCon._lockAllActions = false;
    }
}
