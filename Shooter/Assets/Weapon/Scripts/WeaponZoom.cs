using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float Ergonomics;
    [SerializeField][Range(0.01f, 1f)] private float ZoomSize = 1f;
    private bool isZoom = false;

}
