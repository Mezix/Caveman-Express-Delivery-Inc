using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public Button _button;
    public Image _graphic;

    public AudioSource _hoverSound;
    public AudioSource _clickSound;

    private void Start()
    {
        InitAudio();
    }

    private void InitAudio()
    {
        if(_button && _clickSound) _button.onClick.AddListener(() => Click());
    }

    public void Hover()
    {
        _hoverSound.Play();
    }
    public void Click()
    {
        _clickSound.Play();
    }
}
