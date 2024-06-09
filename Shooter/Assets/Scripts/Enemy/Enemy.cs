using System.Collections.Generic;
using NaughtyAttributes;
using Shooter.Interactable.Buff;
using Shooter.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Shooter.Enemy
{
    public enum TypeOfEnemy
    {
        Default,
        Sprinter,
        Tank,
        Shooter,
        Explosive,
        Toxic,
        Bonus
    }
    public class Enemy : MonoBehaviour
    {
        [FormerlySerializedAs("typesOfEnemy")] public TypeOfEnemy typeOfEnemy;
        [Header("Health")]
        [SerializeField][Range(1, 5000)] private int maxHealth = 100;
        [Range(1, 30)] public float contusionDelay = 3f;
        [Range(1, 30)] public float deathDelay = 3f;
        [HideInInspector] public bool isAlive = true;
        [ReadOnly] public int currentHealth;
        [Space(15)]

        [Min(10)] public int rewardAmount = 100;

        [Space(15)] 
        [SerializeField] [Range(0.01f, 1f)] public float buffsSpawnProbability;
        [SerializeField] public Buff[] buffs;

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