using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action OnShootInput;
    public static Action OnShootInputDown;
    public static Action OnReleasedTrigger;

    public static Action OnReloadInput;
    public static Action OnReloadEnd;

    public static Action<bool> OnZoomInput;

    public static Action OnTakenGun;
    public static Action OnChangedGun;
    public static Action OnDropGun;

    [SerializeField] private KeyCode reloadKey;
    private void Update()
    {
        if (Input.GetMouseButton(0))
            OnShootInput?.Invoke();

        //if (Input.GetMouseButtonUp(0))
        //    OnReleasedTrigger?.Invoke();
        if (Input.GetMouseButtonDown(0))
            OnShootInputDown?.Invoke();

        if (Input.GetMouseButton(1))
            OnZoomInput?.Invoke(true);


        if (Input.GetMouseButtonUp(1))
            OnZoomInput?.Invoke(false);


        if (Input.GetKeyDown(reloadKey))
            OnReloadInput?.Invoke(); 

    }
}
