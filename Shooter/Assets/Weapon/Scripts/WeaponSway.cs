using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivityMultiplier;
    [SerializeField][Range(0.01f, 2f)] private float walkSensitivity = 0.2f;
    [SerializeField][Range(0.01f, 2f)] private float tiltSensitivity = 0.5f;
    private void Update()
    {
        // get mouse input
        float mouseX = (Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal") * walkSensitivity) * sensitivityMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityMultiplier;


        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion rotationZ = Quaternion.AngleAxis(-mouseX * tiltSensitivity, Vector3.forward);

        Quaternion targetRotation = rotationX * rotationY * rotationZ;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
    }


}
