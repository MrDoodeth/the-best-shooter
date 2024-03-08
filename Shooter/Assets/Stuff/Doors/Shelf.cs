using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour, IOpenable
{
    [SerializeField] private Transform shelf;
    [SerializeField][Range(-180f, 180f)] private float openingAngle = 90f;
    [SerializeField] private float openDuration;
    [SerializeField] private bool isOpen = false;
    private Quaternion defaultRotation;

    private void Start()
    {
        shelf = this.transform;
        defaultRotation = shelf.rotation;
    }
    void Update()
    {
        if (isOpen)
        {
            shelf.rotation = Quaternion.Slerp(shelf.rotation, Quaternion.Euler(0f, openingAngle, 0f), openDuration * Time.deltaTime);
        }
        else
        {
            shelf.rotation = Quaternion.Slerp(shelf.rotation, defaultRotation, openDuration * Time.deltaTime);
        }
    }

    public void Open()
    {
        isOpen = !isOpen;
    }
}
