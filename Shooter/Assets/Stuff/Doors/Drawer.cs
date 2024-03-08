using UnityEngine;

public class Drawer : MonoBehaviour, IOpenable
{
    [SerializeField] private Transform drawer;
    [SerializeField] private Vector3 offsetVector;
    [SerializeField] private float openDuration;
    [SerializeField] private bool isOpen = false;
    private Vector3 defaultPosition;

    private void Start()
    {
        drawer = this.transform;
        defaultPosition = drawer.position;
    }
    void Update()
    {
        if (isOpen)
        {
            drawer.position = Vector3.Slerp(drawer.position, defaultPosition + offsetVector, openDuration * Time.deltaTime);
        }
        else
        {
            drawer.position = Vector3.Slerp(drawer.position, defaultPosition, openDuration * Time.deltaTime);
        }
    }

    public void Open()
    {
        isOpen = !isOpen;
    }
}