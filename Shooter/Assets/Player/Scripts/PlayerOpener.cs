using UnityEngine;

public class PlayerOpener : MonoBehaviour
{
    [SerializeField] private KeyCode openKey;
    private void Update()
    {
        if (PlayerRayCaster.isHit)
        {
            var _hit = PlayerRayCaster.HIT;
            IOpenable iOpenableEntity = _hit.transform?.GetComponent<IOpenable>();

            if (Input.GetKeyDown(openKey))
            {
                iOpenableEntity?.Open();
            }
        }
    }
}
