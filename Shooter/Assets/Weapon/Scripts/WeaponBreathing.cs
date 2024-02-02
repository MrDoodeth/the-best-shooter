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
    [Space(10)]

    //Sprint
    [SerializeField] private float sprintingTimeScale;
    [SerializeField] private float sprintingAmplitudeX;
    [SerializeField] private float sprintingAmplitudeY;
    [Space(10)]

    [SerializeField] private Vector3 BreathAmount;
    [SerializeField] private float Shift;
    private Vector3 defaultPosition;
    private bool isZoom = true;

    private void OnValidate()
    {
        defaultPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        PlayerController.OnZoomInput += OnZoom;
    }

    private void OnDisable()
    {
        PlayerController.OnZoomInput -= OnZoom;
    }

    private void GunShake()
    {
        if (fpc.isWalking)
        {
            if (fpc.isSprinting)
            {
                timer += Time.deltaTime * sprintingTimeScale;
                Shake(timer, BreathAmount.x * sprintingAmplitudeX, BreathAmount.y * sprintingAmplitudeY, 1f);
            }
            else if (fpc.isCrouched)
            {
                
            }
            else
            {
                timer += Time.deltaTime * walkingTimeScale;
                Shake(timer, BreathAmount.x * walkingAmplitudeX, BreathAmount.y * walkingAmplitudeY, 1f);
            }         
        }

        timer += Time.deltaTime * stayTimeScale;

        Shake(timer, BreathAmount.x, BreathAmount.y, 1f);

        void Shake(float _timer, float X, float Y, float Z)
        {
            float Xpos = transform.localPosition.x + Mathf.Cos(_timer + Shift) * X;
            float Ypos = transform.localPosition.y + Mathf.Sin(_timer) * Y;
            float Zpos = transform.localPosition.z * Z;

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(Xpos, Ypos, Zpos), 150f * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(Zpos, 0, Xpos), 150f * Time.deltaTime);
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
            GunShake();
        }
        else
        {
            transform.localPosition = defaultPosition;
        }

    }
}
