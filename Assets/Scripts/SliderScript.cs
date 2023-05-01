using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public AudioSource _hoverSound;
    public AudioSource _clickSound;

    public Slider _slider;

    public void Hover()
    {
        _hoverSound.Play();
    }
    public void Click()
    {
        _clickSound.Play();
    }
}
