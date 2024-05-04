using Shooter.Player;
using TMPro;
using UnityEngine;

namespace Shooter.UI
{
    public class DisplayHealth : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private PlayerHealth plBehaviour;
        private string defaultText;
        private void OnEnable()
        {
            PlayerHealth.OnPlayerHealthChanged += DetermineHealth;
            defaultText = healthText.text;
        }

        private void OnDisable()
        {
            PlayerHealth.OnPlayerHealthChanged -= DetermineHealth;
        }

        private void DetermineHealth() => healthText.text = defaultText + plBehaviour.CurrentHealth;
    }
}
