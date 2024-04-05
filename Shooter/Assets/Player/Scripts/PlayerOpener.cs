using UnityEngine;

public class PlayerOpener : MonoBehaviour
{
    [SerializeField] private KeyCode openKey;
    private void Update()
    {
        if (PlayerRayCaster.isHit)
        {
            var _hit = PlayerRayCaster.HIT;
            if (_hit.transform.TryGetComponent(out IOpenable iOpenableEntity) && PlayerRayCaster.HIT.transform != null)
            {
                if (Input.GetKeyDown(openKey))
                {
                    iOpenableEntity.Open();
                }
            }
        }
    }
}
