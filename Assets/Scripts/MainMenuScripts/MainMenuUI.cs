using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public ButtonScript _startButton;

    public void InitButtons()
    {
        _startButton._button.onClick.AddListener(() => StartGame());
    }

    private void StartGame()
    {
        Loader.Load(Loader.Scene.GameScene);
    }
}
