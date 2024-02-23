using TMPro;
using UnityEngine;

public class DisplayPoints : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private PlayerInventory plInventory;

    private void OnEnable()
    {       
        PlayerInventory.OnChangePointsAmount += DeterminePoints;
    }

    private void OnDisable()
    {
        PlayerInventory.OnChangePointsAmount -= DeterminePoints;
    }

    private void DeterminePoints() => pointsText.text = plInventory.pointsAmount.ToString();
}
