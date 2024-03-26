using System;
using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    [Header("Purchase")]
    [SerializeField] private TMP_Text priceLabel;
    [SerializeField] private KeyCode buyKey;
    [SerializeField] private int PointsAmount;
    public static event Action OnChangePointsAmount;
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

            IBuyable iBuyableEntity = _hit.transform.GetComponent<IBuyable>();
            priceLabel.enabled = true;
            priceLabel.text = iBuyableEntity?.price.ToString();

            if (Input.GetKeyDown(buyKey))
            {               
                iBuyableEntity?.Buy(ref PointsAmount);
                OnChangePointsAmount?.Invoke();
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
