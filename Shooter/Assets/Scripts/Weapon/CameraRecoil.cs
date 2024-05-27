using UnityEngine;

namespace Shooter.Weapon
{
    public class CameraRecoil : MonoBehaviour
    {
        [SerializeField] private float returnSpeed;
        [SerializeField] private float snappiness;
        private static Vector3 targetRotation;
        private Vector3 currentRotation;
        void Awake()
        {
        
        }

        private void Update()
        {
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Euler(currentRotation);
        }

        public static void Recoil(Vector3 recoil)
        {
            targetRotation += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
        }
    }
}