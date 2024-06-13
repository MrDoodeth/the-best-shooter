using UnityEngine;

namespace Shooter.Interactable.Door
{
    public class Shelf : MonoBehaviour, IOpenable
    {
        [SerializeField][Range(-180f, 180f)] private float openingAngleX = 0f;
        [SerializeField][Range(-180f, 180f)] private float openingAngleY = 90f;
        [SerializeField][Range(-180f, 180f)] private float openingAngleZ = 0f;
        [SerializeField] private float openDuration = 3f;
        public bool isOpen { get; set; }
        private Quaternion defaultRotation;

        private void Awake()
        {
            defaultRotation = transform.localRotation;
        }

        private void Update()
        {
            if (isOpen)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(openingAngleX, openingAngleY, openingAngleZ), openDuration * Time.deltaTime);
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, defaultRotation, openDuration * Time.deltaTime);
            }
        }
        public void Open() => isOpen = !isOpen;
    }
}
