using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivityMultiplier;
    private void Update()
    {
        // get mouse input
        float mouseX = (Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal") * 0.1f) * sensitivityMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityMultiplier;

        Mathf.Clamp(mouseX, 0f, 1f);
        Mathf.Clamp(mouseY, 0f, 1f);

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion rotationZ = Quaternion.AngleAxis(-mouseX * 0.4f, Vector3.forward);

        Quaternion targetRotation = rotationX * rotationY * rotationZ;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
    }
}
