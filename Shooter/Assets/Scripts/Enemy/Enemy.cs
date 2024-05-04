using System.Collections.Generic;
using Shooter.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Shooter.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField][Range(1, 5000)] private float maxHealth = 100;
        [Range(1, 30)] public float contusionDelay = 3f;
        [Range(1, 30)] public float deathDelay = 3f;
        public bool isAlive = true;
        public float currentHealth;
        [Space(15)]

        [Min(10)] public int rewardAmount = 100;

        [HideInInspector] public Rigidbody[] allRigidbodys;
        [HideInInspector] public Collider[] allColliders;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Animator animator;
        [HideInInspector] public Transform playerTransform;
        [HideInInspector] public PlayerScore playerScore;

        private void Awake()
        {
            Init();
        }
        private void Init()
        {
            allRigidbodys = GetComponentsInChildren<Rigidbody>();
            allColliders = GetComponentsInChildren<Collider>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerTransform = player.transform;
            playerScore = player.GetComponent<PlayerScore>();
            allRigidbodys.SetIsKinematics(true);
            currentHealth = maxHealth;
        }
    }

//Перегрузка метода
    public static class RigidbodyTools
    {
        public static void SetIsKinematics(this IEnumerable<Rigidbody> rigidBodies, bool isKinematic)
        {
            foreach (var rb in rigidBodies)
            {
                rb.isKinematic = isKinematic;
            }
        }
    }
}