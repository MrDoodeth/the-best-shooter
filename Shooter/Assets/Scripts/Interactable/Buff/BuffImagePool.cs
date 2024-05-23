using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Shooter.Interactable.Buff
{
    public class BuffImagePool
    {
        private readonly ObjectPool<Image> pool;
        private readonly Image prefab;
        private readonly Transform buffStatusBarTransform;

        public BuffImagePool(Image prefab, int prewarmObjectsCount, Transform buffStatusBarTransform)
        {
            this.prefab = prefab;
            this.buffStatusBarTransform = buffStatusBarTransform;
            pool = new ObjectPool<Image>(OnImageCreate, OnImageGet, OnImageRelease, OnImageDestroy, false, prewarmObjectsCount, prewarmObjectsCount + 4);
        }

        public Image Get() => pool.Get();
        public void Release(Image obj) => pool.Release(obj);
        
        private void OnImageDestroy(Image obj) => Object.Destroy(obj.gameObject);
        private void OnImageRelease(Image obj) => obj.gameObject.SetActive(false);

        private void OnImageGet(Image obj)
        {
            obj.fillAmount = 1f;
            obj.gameObject.SetActive(true);
        }

        private Image OnImageCreate()
        {
            return Object.Instantiate(prefab, buffStatusBarTransform);
        }
    }
    public struct BuffIconUI
    {
        public Image image;
        public float durationOfBuff;
        public BuffState buffState;
    }
}
