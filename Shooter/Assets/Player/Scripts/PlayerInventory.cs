using System;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("Balance")]
    [SerializeField] private int PointsAmount;
    public static event Action OnChangePointsAmount;

    [Header("Purchase")]
    [SerializeField] private TMP_Text priceLabel;
    public int pointsAmount
    {
        get { return PointsAmount; }
        set
        {
            PointsAmount = Math.Clamp(value, 0, int.MaxValue);
            OnChangePointsAmount?.Invoke();
        }
    }

    private void Awake()
    {
        priceLabel.enabled = false;
    }
    private void Update()
    {
        if (PlayerRayCaster.isHit)
        {
            var _hit = PlayerRayCaster.HIT;

            var iBuyableEntity = _hit.transform.GetComponent<IBuyable>();
            priceLabel.enabled = true;
            priceLabel.text = iBuyableEntity?.price.ToString();

            if (Input.GetKeyDown(KeyCode.F))
            {               
                iBuyableEntity?.Buy(ref PointsAmount);
                OnChangePointsAmount?.Invoke();
                priceLabel.enabled = false;
            }

            var iOpenableEntity = _hit.transform.GetComponent<IOpenable>();

            if (Input.GetKeyDown(KeyCode.F))
            {
                iOpenableEntity?.Open();
            }
        }
        else
        {
            priceLabel.text = "";
            priceLabel.enabled = false;
        }
    }
}
