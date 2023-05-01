using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreenUI : MonoBehaviour
{
    public Text _scoreText;
    public Button returnToMenuButton;
    private void Awake()
    {
        returnToMenuButton.onClick.AddListener(() => Loader.Load(Loader.Scene.MainMenu));
    }
    public void Start()
    {
       if(REF.score) _scoreText.text = REF.score._currentScore.ToString();
    }
}
