using DG.Tweening;
using UnityEngine;

namespace Shooter.Interactable.Door
{
    public class UnlockForPointsDoor : MonoBehaviour, IBuyable
    {
        [SerializeField] private int Price;
        public int price
        {
            get => Price;
            set { Price = value <= 0 ? 0 : value; }
        }
        [Space]
        [SerializeField] private float openDuration = 2f;
        [SerializeField][Range(-180f,180f)] private float openingAngle = 90f;
        [SerializeField] private RotateMode rotMode;
        public int Buy(int balance)
        {
            if (balance < price)
            {
                print("Недостаточно средств!");
                return 0;
            }

            //Usefull actions
            print("Дверь открыта");
            transform.DOLocalRotate(new Vector3(0f, openingAngle, 0f), openDuration, rotMode);
            Destroy(this);

            return price;
        }
    }
}
