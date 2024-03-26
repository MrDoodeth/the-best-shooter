using UnityEngine;

public class Shelf : MonoBehaviour, IOpenable
{
    [SerializeField][Range(-180f, 180f)] private float openingAngle = 90f;
    [SerializeField] private float openDuration;
    [SerializeField] private bool isOpen = false;
    private Quaternion defaultRotation;

    private void Start()
    {
        defaultRotation = transform.localRotation;
    }
    void Update()
    {
        if (isOpen)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, openingAngle, 0f), openDuration * Time.deltaTime);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, defaultRotation, openDuration * Time.deltaTime);
        }
    }

    public void Open()
    {
        isOpen = !isOpen;
    }
}
