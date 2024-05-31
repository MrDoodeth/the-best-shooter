using System;
using UnityEngine;

namespace Shooter.Player
{
    public class PlayerController : MonoBehaviour
    {
        public enum TypeOfClick
        {
            Click,
            Hold
        }

        [SerializeField] private TypeOfClick typeOfClick;
        public static event Action OnAttackInput;
        public static event Action OnAttackInputDown;

        [SerializeField] private KeyCode reloadKey;
        public static event Action OnReloadInput;

        public static event Action<bool> OnZoomInput;
        public static Action OnWeaponTaken;
        public static Action OnWeaponChanged;
        public static Action OnWeaponDroped;

        public static bool isControllerAvailable = true;
        private bool isZoom;

        private void Update()
        {
            if (!isControllerAvailable)
                return;

            if (Input.GetMouseButton(0))
                OnAttackInput?.Invoke();

            if (Input.GetMouseButtonDown(0))
                OnAttackInputDown?.Invoke();

            switch (typeOfClick)
            {
                case TypeOfClick.Click:
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        isZoom = !isZoom;
                        OnZoomInput?.Invoke(isZoom);
                    }
                } break;
                case TypeOfClick.Hold:
                {
                    OnZoomInput?.Invoke(Input.GetMouseButton(1));
                } break;
            }

            if (Input.GetKeyDown(reloadKey))
                OnReloadInput?.Invoke();
        }
    }
}
