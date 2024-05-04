using DG.Tweening;
using Shooter.Interactable.Item;
using Shooter.Player;
using UnityEngine;

namespace Shooter.Interactable.Door
{
    public class UnlockForItemDoor : MonoBehaviour, ITradable
    {
        [field: SerializeField] public Item.Item neededItem { get; set; }
        [field: SerializeField] public int neededItemsAmount { get; set; }
        [Space]
        [SerializeField] private float openDuration = 2f;
        [SerializeField][Range(-180f, 180f)] private float openingAngle = 90f;
        [SerializeField] private RotateMode rotMode;

        public void Trade(PlayerInventory plInv)
        {       
            if (plInv.TryDeleteItems(neededItem, neededItemsAmount))
            {            
                //Usefull actions
                print("Дверь открыта");
                transform.DOLocalRotate(new Vector3(0f, openingAngle, 0f), openDuration, rotMode);
                Destroy(this);
            }
            else
            {
                // Не открылась
            }
        }
    }
}
