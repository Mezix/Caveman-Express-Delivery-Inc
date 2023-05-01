using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public static class Loader {
    public enum Scene { MainMenu, BossOpeningLevel, FirstLvl, LongLevel}
    public static Scene _currentScene;

    public static void Load(Scene scene)
    {
        _currentScene = scene;
        SceneManager.LoadScene(scene.ToString());
    }
}
