using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponRunPosition : MonoBehaviour
{
    [SerializeField] private FirstPersonController fpc;
    [SerializeField] private WeaponSwitching weaponSwitching;
    public Transform runTransform;

    private Transform defaultPosition;



    private void Awake()
    {
        defaultPosition = transform;
    }
    void Update()
    {
        if (fpc != null)
        {
            if (fpc.isWalking)
            {
                if (fpc.isSprinting)
                {
                    transform.DOLocalMove(runTransform.localPosition, 0.7f);
                    transform.DOLocalRotate(runTransform.localRotation.eulerAngles, 0.7f);
                    weaponSwitching.gun.enabled = false;
                    weaponSwitching.enabled = false;
                }
                else
                {
                    transform.DOLocalMove(defaultPosition.localPosition, 0.7f);
                    transform.DOLocalRotate(defaultPosition.localRotation.eulerAngles, 0.7f);
                    weaponSwitching.enabled = true;
                    if (weaponSwitching.gun != null)
                        weaponSwitching.gun.enabled = true;
                }
            }
            else
            {
                transform.DOLocalMove(defaultPosition.localPosition, 0.7f);
                transform.DOLocalRotate(defaultPosition.localRotation.eulerAngles, 0.7f);
                weaponSwitching.enabled = true;
                if (weaponSwitching.gun != null)
                    weaponSwitching.gun.enabled = true;
            }
        }
    }
}
