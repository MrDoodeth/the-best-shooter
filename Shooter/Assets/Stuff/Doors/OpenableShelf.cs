using UnityEngine;

public class OpenableShelf : MonoBehaviour, IOpenable
{
    [SerializeField] private Transform shelf;
    [SerializeField] private Vector3 offsetVector;
    [SerializeField] private float openDuration;
    [SerializeField] private bool isOpen = false;
    private Vector3 defaultPosition;

    private void Start()
    {
        shelf = this.transform;
        defaultPosition = shelf.position;
    }
    void Update()
    {
        if (isOpen)
        {
            shelf.position = Vector3.Slerp(shelf.position, defaultPosition + offsetVector, openDuration * Time.deltaTime);
        }
        else
        {
            shelf.position = Vector3.Slerp(shelf.position, defaultPosition, openDuration * Time.deltaTime);
        }
    }

    public void Open()
    {
        isOpen = !isOpen;
    }
}