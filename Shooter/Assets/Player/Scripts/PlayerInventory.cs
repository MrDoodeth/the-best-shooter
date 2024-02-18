using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int PointsAmount;
    public static event Action OnChangePointsAmount;
    public int pointsAmount
    {
        get { return PointsAmount; }
        set
        {
            PointsAmount = value;
            OnChangePointsAmount?.Invoke();
        }
    }

}
