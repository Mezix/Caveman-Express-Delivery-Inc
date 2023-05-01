using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwap : MonoBehaviour
{
    public Loader.Scene nextScene;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out PlayerController p))
        {
            Debug.Log("Loading : " + nextScene);
            Loader.Load(nextScene);
            if(REF.score) REF.score.SaveTimeAtEndOfStage();
        }
    }
}
