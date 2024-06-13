using UnityEngine;

namespace Shooter.Interactable.Door
{
    public class Drawer : MonoBehaviour, IOpenable
    {
        [SerializeField] private Vector3 offsetVector;
        [SerializeField] private float openDuration = 3f;
        public bool isOpen { get; set; }
        private Vector3 defaultPosition;

        private void Awake()
        {
            defaultPosition = transform.localPosition;
        }

        private void Update()
        {
            if (isOpen)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition + offsetVector, openDuration * Time.deltaTime);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition, openDuration * Time.deltaTime);
            }
        }

        public void Open() => isOpen = !isOpen;
    }
}