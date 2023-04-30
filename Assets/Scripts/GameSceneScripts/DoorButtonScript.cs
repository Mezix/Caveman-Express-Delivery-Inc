using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButtonScript : MonoBehaviour, Punchable
{
    public SpriteRenderer buttonRenderer;
    public DoorScript doorScript;
    public List<PackageReceiver> holeList;
    private bool readyToHit;

    public float timer;
    public float timeSinceOpen;

    public AudioSource buttonPressSound;

    public ButtonState _buttonState;
    public enum ButtonState
    {
        Pressed,
        NotPressed,
        NotPressable
    }

    private void Start()
    {
        timeSinceOpen = timer;
        InitButton();
    }

    private void InitButton()
    {
        if (holeList.Count > 0)
        {
            UpdateButton(ButtonState.NotPressable);
        }
        else
        {
            UpdateButton(ButtonState.NotPressed);
        }
    }

    void Update()
    {
       timeSinceOpen += Time.deltaTime;

        CheckTimerFinished();
        CheckActivatedPackageReceivers();
    }

    private void CheckTimerFinished()
    {
        if (timer <= 0) return;

        if (timeSinceOpen > timer)
        {
            doorScript.OpenDoor(false);
            UpdateButton(ButtonState.NotPressed);
        }
    }


    private void CheckActivatedPackageReceivers()
    {
        List<PackageReceiver> alreadyActivatedHoleList = new List<PackageReceiver>();
        int alreadyActivatedHoles = 0;
        foreach (PackageReceiver hole in holeList)
        {
            if (hole.alreadyActivated)
            {
                alreadyActivatedHoles++;
                alreadyActivatedHoleList.Add(hole);
            }
        }
        if (holeList.Count <= alreadyActivatedHoles)
        {
            readyToHit = true;
            buttonRenderer.color = Color.green;
        }
        foreach (PackageReceiver alrActHol in alreadyActivatedHoleList)
        {
            holeList.Remove(alrActHol);
        }
    }

    public void UpdateButton(ButtonState state)
    {
        if(state == ButtonState.NotPressed)
        {
            buttonRenderer.sprite = Resources.Load(GS.Props("ButtonNotPressed"), typeof(Sprite)) as Sprite;
            readyToHit = true;
            buttonRenderer.color = Color.green;
        }
        else if (state == ButtonState.Pressed)
        {
            buttonRenderer.sprite = Resources.Load(GS.Props("ButtonPressed"), typeof(Sprite)) as Sprite;
            readyToHit = false;
            buttonRenderer.color = Color.white;
        }
        else if(state == ButtonState.NotPressable)
        {
            buttonRenderer.sprite = Resources.Load(GS.Props("ButtonNotPressed"), typeof(Sprite)) as Sprite;
            readyToHit = false;
            buttonRenderer.color = Color.white;
        }

        _buttonState = state;
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PackageScript p) && readyToHit)
        {
            HitButton();
        }
    }

    public void HitButton()
    {
        if (_buttonState == ButtonState.Pressed) return;
        buttonPressSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        buttonPressSound.Play();
        UpdateButton(ButtonState.Pressed);
        doorScript.OpenDoor(true);
        if (timer > 0) timeSinceOpen = 0;
    }

    public void Punched(Vector3 dir)
    {
        HitButton();
    }
}
