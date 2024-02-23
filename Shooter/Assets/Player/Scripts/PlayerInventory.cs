using System;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("Balance")]
    [SerializeField] private int PointsAmount;
    public static event Action OnChangePointsAmount;

    [Header("Purchase")]
    [SerializeField] private float pickUpDistance;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private TMP_Text priceLabel;
    public int pointsAmount
    {
        get { return PointsAmount; }
        set
        {
            PointsAmount = value;
            OnChangePointsAmount?.Invoke();
        }
    }

    private void Awake()
    {
        priceLabel.enabled = false;
    }
    private void Update()
    {
        if (Physics.Raycast(centerPoint.position, transform.forward, out RaycastHit hit, pickUpDistance))
        {           
            if (hit.transform.CompareTag("Vendor"))
            {
                var vendor = hit.transform.GetComponent<IBuyable>();
                priceLabel.enabled = true;
                priceLabel.text = vendor.price.ToString();

                if (Input.GetKeyDown(KeyCode.F))
                {
                    vendor.Buy(ref PointsAmount);
                    OnChangePointsAmount?.Invoke();
                }
            }
            else
            {
                priceLabel.text = "";
                priceLabel.enabled = false;
            }
        }
        else
        {
            priceLabel.text = "";
            priceLabel.enabled = false;
        }
    }
}
