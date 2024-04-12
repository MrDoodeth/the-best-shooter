using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected int damage;
    [field:SerializeField] public string Name { get; protected set; }
    [field: SerializeField] public int Size { get; protected set; }
    [SerializeField][Min(0.01f)] protected float powerOfHit;

    [SerializeField] public Transform rightHandle;
    [SerializeField] public Transform leftHandle;
    protected FirstPersonController fpc;
    protected Animator animator;

    [HideInInspector] public WeaponZoom weaponZoom;
    [HideInInspector] public WeaponSway weaponSway;
    [HideInInspector] public WeaponBobbing weaponBobbing;
    [HideInInspector] public Rigidbody weaponRigidbody;
    [HideInInspector] public BoxCollider weaponCollider;

    protected void Initialization()
    {
        weaponZoom = GetComponent<WeaponZoom>();
        weaponSway = GetComponent<WeaponSway>();
        weaponBobbing = GetComponent<WeaponBobbing>();
        weaponRigidbody = GetComponent<Rigidbody>();
        weaponCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
    }
    public void SetFPC(FirstPersonController fpc)
    {
        this.fpc = fpc;
        weaponZoom.FPC = fpc != null ? fpc : null;
    }
    protected void TurnOnControl()
    {
        weaponZoom.enabled = true;
        weaponSway.enabled = true;
        weaponBobbing.enabled = true;
    }
    protected void TurnOffControl()
    {
        weaponZoom.enabled = false;
        weaponSway.enabled = false;
        weaponBobbing.enabled = false;
    }
}
