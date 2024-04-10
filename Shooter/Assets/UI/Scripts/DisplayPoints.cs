using TMPro;
using UnityEngine;

public class DisplayPoints : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private PlayerScore plInventory;

    private void OnEnable()
    {       
        PlayerScore.OnChangePointsAmount += DeterminePoints;
    }

    private void OnDisable()
    {
        PlayerScore.OnChangePointsAmount -= DeterminePoints;
    }

    private void DeterminePoints() => pointsText.text = plInventory.pointsAmount.ToString();
}
