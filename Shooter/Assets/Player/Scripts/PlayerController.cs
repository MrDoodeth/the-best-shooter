using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TypeOfClick typeOfClick; //Show in Editor
    public static TypeOfClick TypeOfClick;
    public static Action OnShootInput;
    public static Action OnShootInputDown;

    public static Action OnReloadInput;
    public static Action OnReloadEnd;

    public static Action<bool> OnZoomInput;

    public static Action OnTakenGun;
    public static Action OnChangedGun;
    public static Action OnDropGun;

    public static bool isControllerAvailable;
    private bool isZoom;

    [SerializeField] private KeyCode reloadKey;


    private void Start()
    {
        TypeOfClick = typeOfClick;
    }
    private void Update()
    {
        if (!isControllerAvailable)
            return;

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
                        OnZoomInput?.Invoke(!isZoom);
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
