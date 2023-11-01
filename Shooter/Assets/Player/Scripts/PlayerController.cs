using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TypeOfClick typeOfClick;
    public static TypeOfClick TypeOfClick;

    public static Action OnShootInput;
    public static Action OnShootInputDown;

    public static Action OnReloadInput;
    public static Action OnReloadEnd;

    public static Action<bool> OnZoomInput;
    public static Action<bool> OnZoomInputDown;

    public static Action OnTakenGun;
    public static Action OnChangedGun;
    public static Action OnDropGun;

    private bool isZoom;

    [SerializeField] private KeyCode reloadKey;

    private void Start()
    {
        TypeOfClick = typeOfClick;
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
            OnShootInput?.Invoke();

        if (Input.GetMouseButtonDown(0))
            OnShootInputDown?.Invoke();

        switch (TypeOfClick)
        {
            case TypeOfClick.Click:
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        OnZoomInputDown?.Invoke(!isZoom);
                        isZoom = !isZoom;
                    }
                }
                break;
            case TypeOfClick.Hold:
                {
                    if (Input.GetMouseButton(1))
                        OnZoomInput?.Invoke(true);
                    else
                        OnZoomInput?.Invoke(false);
                }
                break;
        }

        if (Input.GetKeyDown(reloadKey))
            OnReloadInput?.Invoke(); 

    }
}
