using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField][Range(0.01f, 10f)] private float speed;
    [SerializeField][Range(0.01f, 10f)] private float sensitivityMultiplier;
    [SerializeField][Range(0.01f, 10f)] private float walkSensitivity = 0.1f;

    private void Update()
    {
        // get mouse input
        float mouseX = (Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal") * walkSensitivity) * sensitivityMultiplier;
        float mouseY = -Input.GetAxis("Mouse Y") * sensitivityMultiplier;
        float mouseZ = -Input.GetAxis("Vertical") * sensitivityMultiplier;

        Vector3 targetPosition = new Vector3(mouseX, 0, 0f);

        transform.localPosition = Vector3.Slerp(transform.localPosition, targetPosition, speed * Time.deltaTime);
    }
}
