using UnityEngine;

[RequireComponent(typeof(WeaponZoom), (typeof(WeaponSway)), typeof(WeaponBobbing))]
public sealed class Melee : Weapon
{
    [SerializeField] private float radius;
    [SerializeField] private Transform centerOfSphere;

    private void Awake()
    {
        Initialization();
        //Важно в конце!!!
        enabled = false;
    }
    private void OnEnable()
    {
        PlayerController.OnAttackInputDown += StartHit;
        TurnOnControl();
        animator.enabled = false;
    }
    private void OnDisable()
    {
        PlayerController.OnAttackInputDown -= StartHit;
        TurnOffControl();
    }

    private void StartHit()
    {
        Debug.Log("Hit");
    }
    private void OnDrawGizmos()
    {
        if (centerOfSphere != null)
            Gizmos.DrawWireSphere(centerOfSphere.position, radius);
    }
}
