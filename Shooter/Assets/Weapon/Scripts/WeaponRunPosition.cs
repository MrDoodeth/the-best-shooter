using UnityEngine;

public class WeaponRunPosition : MonoBehaviour
{
    [SerializeField] private FirstPersonController fpc;
    [SerializeField] private WeaponSwitching weaponSwitching;
    public float speed;
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
                    //transform.localPosition = Vector3.Lerp(transform.localPosition, runTransform.localPosition, speed * Time.deltaTime);
                    //transform.localRotation = Quaternion.Lerp(transform.localRotation, runTransform.localRotation, speed * Time.deltaTime);

                    //transform.DOLocalMove(runTransform.localPosition, 0.7f); 
                    //transform.DOLocalRotate(runTransform.localRotation.eulerAngles, 0.7f);

                    transform.localPosition = runTransform.localPosition;
                    transform.localRotation = runTransform.localRotation;
                }
                else
                {
                    //transform.DOLocalMove(defaultPosition.localPosition, 0.7f);
                    //transform.DOLocalRotate(defaultPosition.localRotation.eulerAngles, 0.7f);

                    //transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition.localPosition, speed * Time.deltaTime);
                    //transform.localRotation = Quaternion.Lerp(transform.localRotation, defaultPosition.localRotation, speed * Time.deltaTime);

                    transform.localPosition = defaultPosition.localPosition;
                    transform.localRotation = defaultPosition.localRotation;
                }
            }
            else
            {
                //transform.DOLocalMove(defaultPosition.localPosition, 0.7f);
                //transform.DOLocalRotate(defaultPosition.localRotation.eulerAngles, 0.7f);
            }
        }
    }
}
