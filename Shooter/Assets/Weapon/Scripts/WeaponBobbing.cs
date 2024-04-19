using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivityMultiplier;
    [SerializeField] private float walkSensitivity = 0.1f;

    private Vector3 defaultWeaponPosition;

    private void Awake()
    {
        defaultWeaponPosition = transform.position;
    }

    private void Update()
    {
        // get mouse input
        float mouseX = (Input.GetAxis("Horizontal") * walkSensitivity) * sensitivityMultiplier;
        float mouseZ = (Input.GetAxis("Vertical") * walkSensitivity) * sensitivityMultiplier;

        Vector3 targetPosition = new Vector3(mouseX, mouseZ, 0f) + transform.position;

        transform.localPosition = Vector3.Slerp(transform.localPosition, targetPosition, speed * Time.deltaTime);

        transform.localPosition = Vector3.Slerp(transform.localPosition, defaultWeaponPosition, speed * 0.5f * Time.deltaTime);
    }
}
