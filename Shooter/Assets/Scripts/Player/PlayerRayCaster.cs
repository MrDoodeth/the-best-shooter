using UnityEngine;

namespace Shooter.Player
{
    public class PlayerRayCaster : MonoBehaviour
    {
        public Transform centerPoint;
        [Range(0.01f, 10f)] public float pickUpDistance;
        public static RaycastHit HIT;
        public static bool isHit;

        private void Update()
        {
            isHit = Physics.Raycast(centerPoint.position, transform.forward, out RaycastHit hit, pickUpDistance);
            HIT = hit;
            Debug.DrawRay(centerPoint.position, transform.forward, Color.green);
        }
    }
}
