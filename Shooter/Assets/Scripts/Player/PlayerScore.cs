using System;
using TMPro;
using UnityEngine;
using Shooter.Interactable;

namespace Shooter.Player
{
    public class PlayerScore : MonoBehaviour
    {
        [Header("Purchase")]
        [SerializeField] private KeyCode buyKey;
        [SerializeField] private TMP_Text priceLabel;
        [SerializeField] private int PointsAmount;
        public static Action OnChangePointsAmount;
        public static Action<int> OnGetReward;
        public int pointsAmount
        {
            get => PointsAmount;
            private set
            {
                PointsAmount = Math.Clamp(value, 0, int.MaxValue);
                OnChangePointsAmount?.Invoke();
            }
        }
        private void Awake()
        {
            priceLabel.enabled = false;
        }
        private void OnEnable()
        {
            OnGetReward += Reward;
        }
        private void OnDisable()
        {
            OnGetReward -= Reward;
        }
        private void Reward(int value)
        {
            pointsAmount += value;
        }
        private void Update()
        {
            if (PlayerRayCaster.isHit && PlayerRayCaster.HIT.transform is not null)
            {
                var hit = PlayerRayCaster.HIT;
                if (hit.transform.TryGetComponent(out IBuyable iBuyableEntity))
                {
                    priceLabel.enabled = true;
                    priceLabel.text = iBuyableEntity?.price.ToString();

                    if (Input.GetKeyDown(buyKey))
                    {
                        pointsAmount -= iBuyableEntity.Buy(pointsAmount);
                        priceLabel.enabled = false;
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
}
