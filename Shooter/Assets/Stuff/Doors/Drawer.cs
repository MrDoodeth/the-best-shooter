using UnityEngine;

public class Drawer : MonoBehaviour, IOpenable
{
    [SerializeField] private Vector3 offsetVector;
    [SerializeField] private float openDuration;
    [SerializeField] private bool isOpen = false;
    private Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = transform.position;
    }
    void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.Slerp(transform.position, defaultPosition + offsetVector, openDuration * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, defaultPosition, openDuration * Time.deltaTime);
        }
    }

    public void Open()
    {
        isOpen = !isOpen;
    }
}