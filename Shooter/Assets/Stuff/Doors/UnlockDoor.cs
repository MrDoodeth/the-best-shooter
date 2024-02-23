using DG.Tweening;
using UnityEngine;

public class UnlockDoor : MonoBehaviour, IBuyable
{
    [SerializeField] private int Price;
    [SerializeField] private float openDuration = 2f;
    [SerializeField][Range(-180f,180f)] private float openingAngle = 90f;
    [SerializeField] private RotateMode rotMode;
    public int price
    {
        get => Price;
        set { Price = value <= 0 ? 0 : value; }
    }
    public void Buy(ref int balance)
    {
        if (balance < price)
        {
            Debug.Log("Недостаточно средств!");
            return;
        }

        balance -= price;
        Debug.Log("Дверь открыта");
        transform.DORotate(new Vector3(0f, openingAngle, 0f), openDuration, rotMode);
        Destroy(this);

    }
}
