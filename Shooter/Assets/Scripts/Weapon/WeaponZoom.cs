using System;
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
        [HideInInspector] public FirstPersonController fpc;
        private WeaponSwayAndBobbing weaponSwayAndBobbing;
        public bool isZoom = false;

        private void Awake()
        {
            weaponSwayAndBobbing = GetComponent<WeaponSwayAndBobbing>();
        }

        private void OnEnable()
        {
            PlayerController.OnZoomInput += Zoom;
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
                fpc.zoomFOV = fpc.fov * zoomSize;

            isZoom = isPressed;
        }

        private void Update()
        {
            if (isZoom && !fpc.isSprinting)
            {
                weaponSwayAndBobbing.bob0ffset = false;
                weaponSwayAndBobbing.bobSway = false;
                targetPosition = zoomPosition;
            }
            else
            {
                weaponSwayAndBobbing.bob0ffset = true;
                weaponSwayAndBobbing.bobSway = true;
                targetPosition = defaultWeaponPosition;
            }
        }
    }
}
