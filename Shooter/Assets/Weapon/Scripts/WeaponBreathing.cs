using System.Threading;
using UnityEngine;

public class WeaponBreathing : MonoBehaviour
{
    [SerializeField] private FirstPersonController fpc;
    [SerializeField] private float timer;

    [Header("OnTimerChange")]
    //Stay
    [SerializeField] private float stayTimeScale;
    [Space(10)]

    //Walk
    [SerializeField] private float walkingTimeScale;
    [SerializeField] private float walkingAmplitudeX;
    [SerializeField] private float walkingAmplitudeY;
    [SerializeField] private float walkingAmplitudeZ;
    [Space(10)]

    //Sprint
    [SerializeField] private float sprintingTimeScale;
    [SerializeField] private float sprintingAmplitudeX;
    [SerializeField] private float sprintingAmplitudeY;
    [SerializeField] private float sprintingAmplitudeZ;
    [Space(10)]

    [SerializeField] private Vector3 BreathAmount;
    [SerializeField] private float Shift;
    private Vector3 defaultLocalPosition;
    private bool isZoom = true;

    private void OnValidate()
    {
        defaultLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        PlayerController.OnZoomInput += OnZoom;
    }

    private void OnDisable()
    {
        PlayerController.OnZoomInput -= OnZoom;
    }

    private void GunShaking()
    {
        if (fpc.isWalking)
        {
            if (fpc.isSprinting)
            {
                timer += Time.deltaTime * sprintingTimeScale;
                ShakeRotation(timer, BreathAmount.x * sprintingAmplitudeX, BreathAmount.y * sprintingAmplitudeY, BreathAmount.z * sprintingAmplitudeZ);
            }
            else if (fpc.isCrouched)
            {
                
            }
            else
            {
                timer += Time.deltaTime * walkingTimeScale;
                ShakeRotation(timer, BreathAmount.x * walkingAmplitudeX, BreathAmount.y * walkingAmplitudeY, BreathAmount.z * walkingAmplitudeZ);
            }         
        }

        timer += Time.deltaTime * stayTimeScale;

        ShakeRotation(timer, BreathAmount.x, BreathAmount.y, BreathAmount.z);

        void Shake(float _timer, float X, float Y, float Z)
        {
            float Xpos = transform.localPosition.x + Mathf.Sin(_timer + Shift) * X;
            float Ypos = transform.localPosition.y + Mathf.Sin(_timer) * Y;
            float Zpos = transform.localPosition.z * Z;

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(Xpos, Ypos, Zpos), 1 / Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, Xpos * 90f), 1 / Time.deltaTime);
        }

        void ShakeRotation(float _timer, float X, float Y, float Z)
        {
            float Xpos = transform.localPosition.x + Mathf.Sin(_timer + Shift) * X;
            float Ypos = transform.localPosition.y + Mathf.Sin(_timer) * Y;
            float Zpos = transform.localPosition.z * Mathf.Sin(_timer) * Z;

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(Xpos, Ypos, Zpos), 1 / Time.deltaTime);
        }
    }

    private void OnZoom(bool _isZoom)
    {
        isZoom = _isZoom;
    }

    private void Update()
    {
        if (!isZoom)
        {
            GunShaking();
        }
        else
        {
            transform.localPosition = defaultLocalPosition;
        }

    }
}
