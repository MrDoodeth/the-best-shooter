using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponRunPosition : MonoBehaviour
{
    [SerializeField] private FirstPersonController fpc;
    public Transform runTransform;
    void Update()
    {
        if (fpc != null)
        {
            if (fpc.isWalking)
            {
                if (fpc.isSprinting)
                {
                    //transform.localPosition = Vector3.Slerp(transform.localPosition, runTransform.position, ergonomics * Time.deltaTime);
                    //transform.localRotation = Quaternion.Slerp(transform.localRotation, runTransform.rotation, ergonomics * Time.deltaTime);

                    transform.localPosition = runTransform.position;
                    transform.localRotation = runTransform.rotation;
                    return;
                }
            }
        }
    }
}
