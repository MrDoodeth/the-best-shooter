using UnityEngine;

public class PlayerRayCaster : MonoBehaviour
{
    [SerializeField] private Transform centerPoint;
    [SerializeField][Range(0.01f, 10f)] private float pickUpDistance;
    public static RaycastHit HIT;
    public static bool isHit;
    void Update()
    {
        isHit = Physics.Raycast(centerPoint.position, transform.forward, out RaycastHit hit, pickUpDistance);
        HIT = hit;
        Debug.DrawRay(centerPoint.position, transform.forward, Color.green);
    }
}
