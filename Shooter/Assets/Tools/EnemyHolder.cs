using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    private Transform body;
    void Start()
    {
        body = GetComponentInParent<Transform>();
    }

    void Update()
    {
        
    }
}
