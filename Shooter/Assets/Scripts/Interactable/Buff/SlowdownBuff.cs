using Shooter.Player;
using UnityEngine;

namespace Shooter.Interactable.Buff
{
    public class SlowdownBuff : Buff
    {
        [SerializeField, Range(0.01f, 0.99f)] private float slowdownRatio;
        [SerializeField] private float lerpSpeed;
        [SerializeField, Range(0.01f, 20f)] private float duration;
        [SerializeField] private Sprite icon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                PlayerBuffs.OnSlowdownBuffActivated?.Invoke(BuffState.Slowdown, icon, duration, slowdownRatio, lerpSpeed);
                print("___SLOWDOWN___");
                Destroy(gameObject);
            }
        }
    }
}
