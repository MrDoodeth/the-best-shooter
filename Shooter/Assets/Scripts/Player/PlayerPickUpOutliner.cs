using UnityEngine;

namespace Shooter.Player
{
    public class PlayerPickUpOutliner : MonoBehaviour
    {
        private PickUpOutline previousPickUpOutline;
        private void Update()
        {
            if (PlayerRayCaster.isHit)
            {
                if (previousPickUpOutline != null)
                    previousPickUpOutline.OnHoverExit();
                
                var hit = PlayerRayCaster.HIT;
                if (hit.transform.TryGetComponent(out PickUpOutline pickUpOutline))
                {
                    previousPickUpOutline = pickUpOutline;
                    previousPickUpOutline.OnHoverEnter();
                }
                else if (previousPickUpOutline != null)
                {
                    previousPickUpOutline.OnHoverExit();
                    previousPickUpOutline = null;
                }
            }
            else if (previousPickUpOutline != null)
            {
                previousPickUpOutline.OnHoverExit();
                previousPickUpOutline = null;
            }
        }
    }
}
