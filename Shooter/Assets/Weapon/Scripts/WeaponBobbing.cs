using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField][Range(0.01f, 10f)] private float speed;
    [SerializeField][Range(0.01f, 10f)] private float sensitivityMultiplier;
    [SerializeField][Range(0.01f, 10f)] private float walkSensitivity = 0.1f;
    [HideInInspector] public Vector3 targetPosition;

    private void Update()
    {
        // get mouse input
        float mouseX = (Input.GetAxis("Horizontal") * walkSensitivity) * sensitivityMultiplier;
        float mouseZ = (Input.GetAxis("Vertical") * walkSensitivity) * sensitivityMultiplier;

        targetPosition = new Vector3(mouseX, 0f, 0f);
    }
}
