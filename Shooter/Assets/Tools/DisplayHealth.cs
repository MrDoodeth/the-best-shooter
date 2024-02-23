using TMPro;
using UnityEngine;

public class DisplayHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private PlayerBehaviour plBehaviour;
    private string defaultText;
    private void OnEnable()
    {
        PlayerBehaviour.OnPlayerHealthChanged += DetermineHealth;
        defaultText = healthText.text;
    }

    private void OnDisable()
    {
        PlayerBehaviour.OnPlayerHealthChanged -= DetermineHealth;
    }

    private void DetermineHealth() => healthText.text = defaultText + plBehaviour.CurrentHealth.ToString();
}
