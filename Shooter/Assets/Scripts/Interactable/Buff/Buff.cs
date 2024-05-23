using UnityEngine;

namespace Shooter.Interactable.Buff
{
    public class Buff : MonoBehaviour
    {
        [SerializeField, Range(0.01f, 20f)] private float lifeTime;
        private void Destroy() => Destroy(gameObject);
        private void Start()
        {
            Invoke(nameof(Destroy), lifeTime);
        }
    }

    public enum BuffState
    {
        Fire,
    }
}
