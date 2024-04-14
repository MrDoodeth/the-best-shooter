using UnityEngine;

[RequireComponent(typeof(WeaponZoom), (typeof(WeaponSway)))]
public sealed class Melee : Weapon
{
    [Space(10)]
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
        PlayerController.isControllerAvailable = false;
        animator.enabled = true;
        animator.SetTrigger("isHit");
    }

    private void Hit()
    {
        Collider[] hitColliders = Physics.OverlapSphere(centerOfSphere.position, radius, enemiesLayer);
        foreach (var hitCollider in hitColliders)
        {
            var entity = hitCollider.transform.GetComponentInParent<IDamagable>();
            entity?.TakeDamage(damage);
            if (hitCollider.transform.TryGetComponent<Rigidbody>(out var rb))
                rb.AddForceAtPosition(centerOfSphere.forward * powerOfHit, hitCollider.transform.position, ForceMode.Impulse);
        }
    }

    private void AnimatorOff()
    {
        animator.enabled = false;
        PlayerController.isControllerAvailable = true;
    }
    private void OnDrawGizmos()
    {
        if (centerOfSphere != null)
            Gizmos.DrawWireSphere(centerOfSphere.position, radius);
    }
}
