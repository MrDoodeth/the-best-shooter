using UnityEngine;

namespace Shooter.Enemy
{
     [RequireComponent(typeof(Enemy))]
    public class ToxicEnemyHealth : EnemyHealth
    {
        [SerializeField] private ToxicArea toxicArea;
        [SerializeField] private int toxicAreaDamage;
        [SerializeField] private float toxicAreaDuration;
        private ToxicArea currentToxicArea;

        private void Start()
        {
            currentToxicArea = Instantiate(toxicArea, transform.position, Quaternion.identity);
            currentToxicArea.gameObject.SetActive(false);
            currentToxicArea.damageFrequency = toxicAreaDamage;
            currentToxicArea.duration = toxicAreaDuration;
        }

        private void SpillPoison()
        {
            currentToxicArea.transform.position = transform.position;
            currentToxicArea.gameObject.SetActive(true);
            currentToxicArea.Deactivate();
        }
        
        public override void Die()
        {
            SpillPoison();
            base.Die();
        }  
    }
}
