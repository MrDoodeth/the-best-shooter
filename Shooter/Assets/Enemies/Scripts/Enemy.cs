using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField][Range(1, 5000)] private float maxHealth = 100;
    [Range(1, 30)] public float contusionDelay = 3f;
    [Range(1, 30)] public float deathDelay = 3f;
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public float currentHealth;
    [Space(15)]

    [HideInInspector] public Rigidbody[] allRigidbodys;
    [HideInInspector] public Collider[] allColliders;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform playerPosition;
    void Awake()
    {
        Init();
    }

    private void Init()
    {
        allRigidbodys = GetComponentsInChildren<Rigidbody>();
        allColliders = GetComponentsInChildren<Collider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        allRigidbodys.SetIsKinematics(true);
        currentHealth = maxHealth;
    }
}



//Перегрузка метода
public static class RigidbodyTools
{
    public static void SetIsKinematics(this Rigidbody[] rb, bool _isKinematic)
    {
        for (int i = 0; i < rb.Length; i++)
        {
            rb[i].isKinematic = _isKinematic;
        }
    }
}