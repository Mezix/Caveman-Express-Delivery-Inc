using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public Button _button;
    public Text _text;
    public Image _graphic;

    public AudioSource _hoverSound;
    public AudioSource _clickSound;

    void Update()
    {
        InitAudio();
    }

    private void InitAudio()
    {
        if(_button && _clickSound) _button.onClick.AddListener(() => _clickSound.Play());
        //if(_button && _hoverSound) _button.OnPointerEnter(_hoverSound.Play());
    }
}
