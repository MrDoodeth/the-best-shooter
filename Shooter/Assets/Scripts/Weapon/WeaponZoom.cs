using Shooter.Player;
using UnityEngine;

namespace Shooter.Weapon
{
    public class WeaponZoom : MonoBehaviour
    {
        [Header("Zoom")]
        [SerializeField][Range(0.01f, 1f)] private float zoomSize = 1f;
        public Vector3 zoomPosition;
        public Vector3 defaultWeaponPosition;
        [HideInInspector] public Vector3 targetPosition;
        private WeaponSwayAndBobbing weaponSwayAndBobbing;
        public bool isZoom = false;
        private Transform model;

        private void Awake()
        {
            weaponSwayAndBobbing = GetComponent<WeaponSwayAndBobbing>();
            model = transform.GetChild(0);
        }

        private void OnEnable()
        {
            PlayerController.OnZoomInput += Zoom;
            model.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            PlayerController.OnZoomInput -= Zoom;    
            transform.localPosition += -Vector3.up / 2;
            Zoom(false);
        }
        private void Zoom(bool isPressed)
        {
            if (enabled)
                WeaponsSwitcher.FPC.zoomFOV = WeaponsSwitcher.FPC.fov * zoomSize;

            isZoom = isPressed;
        }

        private void Update()
        {
            if (isZoom && !WeaponsSwitcher.FPC.isSprinting)
            {
                weaponSwayAndBobbing.bob0ffset = false;
                weaponSwayAndBobbing.bobSway = false;
                targetPosition = zoomPosition;
                
                if (WeaponsSwitcher.currentWeapon is not null &&
                    WeaponsSwitcher.currentWeapon is Gun { typeOfGun: TypeOfGun.SniperRifle })
                    model.gameObject.SetActive(false);
            }
            else
            {
                weaponSwayAndBobbing.bob0ffset = true;
                weaponSwayAndBobbing.bobSway = true;
                targetPosition = defaultWeaponPosition;
                
                model.gameObject.SetActive(true);
            }
        }
    }
}
