using Shooter.Player;
using TMPro;
using UnityEngine;

namespace Shooter.UI
{
    public class DisplayPoints : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointsText;
        [SerializeField] private PlayerScore playerScore;

        private void OnEnable()
        {       
            PlayerScore.OnChangePointsAmount += DeterminePoints;
        }

        private void OnDisable()
        {
            PlayerScore.OnChangePointsAmount -= DeterminePoints;
        }

        private void DeterminePoints() => pointsText.text = playerScore.pointsAmount.ToString();
    }
}
