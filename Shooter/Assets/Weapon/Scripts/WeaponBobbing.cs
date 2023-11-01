using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField][Range(0.01f, 2f)] private float speed;
    [SerializeField][Range(0.01f, 2f)] private float sensitivityMultiplier;

    private void Update()
    {
        // get mouse input
        float mouseX = (Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal") * 0.2f) * sensitivityMultiplier;
        float mouseY = (Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical") * 0.2f) * sensitivityMultiplier;

        Mathf.Clamp(mouseX, 0f, 1f);
        Mathf.Clamp(mouseY, 0f, 1f);

        Vector3 targetPosition = new Vector3(mouseX, mouseY, 0f);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, speed * Time.deltaTime);
    }
}
