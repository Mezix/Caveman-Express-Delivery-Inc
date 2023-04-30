using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingArm : MonoBehaviour
{
    public Transform _fist;
    private Animator anim;
    public AudioSource _punchSound;

    [HideInInspector] public float currentCooldown;
    [HideInInspector] public float punchCooldown;

    public PunchState _punchState;

    private List<Punchable> _punchableObjects = new List<Punchable>();
    public enum PunchState
    {
        ReadyToPunch,
        PunchStarted,
        OnCooldown
    }
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        _punchState = PunchState.ReadyToPunch;
        ShowFist(false);
        currentCooldown = 0;
        punchCooldown = 25f;
    }
    void Update()
    {
        RotateArm();
        UpdatePunchState();
    }

    private void RotateArm()
    {
        HM.RotateLocalTransformToAngle(transform, new Vector3 (0,0, HM.GetAngle2DBetween(REF.pCon.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) - 180));
    }

    private void UpdatePunchState()
    {
        if (Input.GetMouseButtonDown(1) && _punchState == PunchState.ReadyToPunch)
        {
            _punchState = PunchState.PunchStarted;
        }
        else if (currentCooldown <= 0 && _punchState == PunchState.OnCooldown)
        {
            _punchState = PunchState.ReadyToPunch;
            ShowFist(false);
        }
    }

    private void FixedUpdate()
    {
        if(currentCooldown > 0) currentCooldown--;
        if(_punchState == PunchState.PunchStarted)
        {
            StartPunch();
        }
    }

    public void ShowFist(bool show)
    {
        _fist.gameObject.SetActive(show);
    }
    public void StartPunch()
    {
        currentCooldown = punchCooldown;
        _punchState = PunchState.OnCooldown;
        ShowFist(true);
        anim.SetTrigger("Punch");

        _punchSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        _punchSound.Play();

        Vector3 punchDir = Input.mousePosition;
        punchDir = Camera.main.ScreenToWorldPoint(punchDir);
        punchDir = punchDir - transform.position;
        punchDir.z = 0f;
        punchDir.Normalize();
        foreach (Punchable p in _punchableObjects)
        {
            p.Punched(punchDir);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Punchable p))
        {
            _punchableObjects.Add(p);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Punchable p))
        {
            _punchableObjects.Remove(p);
        }
    }
}
