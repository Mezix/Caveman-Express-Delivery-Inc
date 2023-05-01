using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public static class Loader
{
    public enum Scene { MainMenu, BossOpeningLevel, FirstLvl, BoxLvl, PrecisionLvl, TwoThrowLvl, DeathLvl, BossLvl, ScoreScreen }
    public static Scene _currentScene = Scene.MainMenu;

    public static void Load(Scene sceneToSwapTo)
    {
        if (_currentScene != Scene.MainMenu && sceneToSwapTo == Scene.MainMenu)
        {
            REF.audio.FadeFromGameToMenu();
        }
        else if (_currentScene == Scene.MainMenu && sceneToSwapTo != Scene.MainMenu)
        {
            if (REF.audio)
            {
                REF.audio.FadeFromMenuToGame();
            }
        }
        _currentScene = sceneToSwapTo;
        SceneManager.LoadScene(sceneToSwapTo.ToString());
    }
}
