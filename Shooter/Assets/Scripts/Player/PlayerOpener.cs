using UnityEngine;
using Shooter.Interactable;

namespace Shooter.Player
{
    public class PlayerOpener : MonoBehaviour
    {
        [SerializeField] private KeyCode openKey;
        private void Update()
        {
            if (PlayerRayCaster.isHit)
            {
                var hit = PlayerRayCaster.HIT;
                if (hit.transform.TryGetComponent(out IOpenable iOpenableEntity) && PlayerRayCaster.HIT.transform is not null)
                {
                    if (Input.GetKeyDown(openKey))
                    {
                        iOpenableEntity.Open();
                    }
                }
            }
        }
    }
}
