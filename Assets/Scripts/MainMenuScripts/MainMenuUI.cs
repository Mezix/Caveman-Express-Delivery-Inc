using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public ButtonScript _startButton;
    public ButtonScript _closeSettingsButton;
    public ButtonScript _quitButton;


    //  Settings
    public ButtonScript _settingsButton;
    public GameObject _settingsParent;
    public bool _settingsShown;
    private void Awake()
    {
        InitButtons();
    }

    void Start()
    {
        ShowSettings(false);
    }
    public void InitButtons()
    {
        _startButton._button.onClick.AddListener(StartGame);
        _settingsButton._button.onClick.AddListener(ToggleSettings);
        _closeSettingsButton._button.onClick.AddListener(() => ShowSettings(false));
        _quitButton._button.onClick.AddListener(QuitGame);
    }
    private void ToggleSettings()
    {
        ShowSettings(!_settingsShown);
    }

    public void ShowSettings(bool settings)
    {
        _settingsShown = settings;
        _settingsParent.SetActive(settings);
    }

    private void StartGame()
    {
        Loader.Load(Loader.Scene.BossOpeningLevel);
    }
    private void QuitGame()
    {
        Application.Quit();
    }
}
