using UnityEngine;
using Shooter.Interactable;
using TMPro;

namespace Shooter.Player
{
    public class PlayerOpener : MonoBehaviour
    {
        [SerializeField] private KeyCode openKey;
        [SerializeField] private TMP_Text label;
        [SerializeField] private string openText;
        [SerializeField] private string closeText;
        private void Update()
        {
            if (PlayerRayCaster.isHit)
            {
                var hit = PlayerRayCaster.HIT;
                if (hit.transform.TryGetComponent(out IOpenable iOpenableEntity))
                {
                    label.enabled = true;
                    if (iOpenableEntity.isOpen)
                        label.text = openText + $"[{openKey.ToString()}]";
                    else
                        label.text = closeText + $"[{openKey.ToString()}]";;
                    
                    
                    if (Input.GetKeyDown(openKey))
                    {
                        iOpenableEntity.Open();
                    }
                }
                else
                {
                    label.text = "";
                    label.enabled = false;
                }
            }
            else
            {
                label.text = "";
                label.enabled = false;
            }
        }
    }
}
