using System;
using System.Collections.Generic;
using Shooter.Interactable.Buff;
using Shooter.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.Player
{
    public class PlayerBuffs : MonoBehaviour
    {
        [SerializeField] private Image imagePrefab;
        [SerializeField] private int imageCount;
        [SerializeField] private Transform buffStatusBarTransform;
        [SerializeField, Range(0.01f, 0.99f)] private float slowdownRatio;
        [SerializeField] private float lerpSpeed;

        private FirstPersonController FPC;
        private PlayerHealth playerHealth;
        private BuffImagePool imagePool;
        private List<BuffIconUI> buffIcons;

        public static Action<BuffState, Sprite, float, int> OnFireBuffActivated;
        public static Action<int> OnHealBuffActivated;
        public static Action<int> OnAmmoBuffActivated;

        private void Awake()
        {
            FPC = GetComponent<FirstPersonController>();
            playerHealth = GetComponent<PlayerHealth>();

            buffIcons = new List<BuffIconUI>();
            
            imagePool = new BuffImagePool(imagePrefab, imageCount, buffStatusBarTransform);
        }
        private void OnEnable()
        {
            OnHealBuffActivated += OnHealBuff;
            OnAmmoBuffActivated += OnAmmoBuff;
            OnFireBuffActivated += OnFireBuff;
        }
        private void OnDisable()
        {
            OnHealBuffActivated -= OnHealBuff;
            OnAmmoBuffActivated -= OnAmmoBuff;
            OnFireBuffActivated -= OnFireBuff;
        }
        private void OnHealBuff(int healAmount) => playerHealth.CurrentHealth += healAmount;
        private void OnAmmoBuff(int ammoAmount)
        {
            if (WeaponsSwitcher.currentWeapon is Gun gun)
            {
                gun.ReserveAmmo += ammoAmount;
            }
        }

        private void OnFireBuff(BuffState buffState, Sprite sprite, float duration, int damage)
        {
            AddBuff(buffState, sprite, duration);
        }

        private void AddBuff(BuffState buffState, Sprite sprite, float duration)
        {
            if (buffIcons.Count > 0)
            {
                for (int i = 0; i < buffIcons.Count; i++)
                {
                    if (buffIcons[i].buffState == buffState)
                    {
                        buffIcons[i].image.fillAmount = 1f;
                        buffIcons[i].image.gameObject.SetActive(true);
                        return;
                    }
                }
            }
            
            var image = imagePool.Get();
            image.sprite = sprite;

            var iconUI = new BuffIconUI()
            {
                image = image,
                buffState = buffState,
                durationOfBuff = duration
            };
            
            buffIcons.Add(iconUI);
        }

        private void UpdateBuffUI()
        {
            if (buffIcons is null) return;
            for (int i = 0; i < buffIcons.Count; i++)
            {
                buffIcons[i].image.fillAmount -= 1.0f / buffIcons[i].durationOfBuff * Time.deltaTime;

                if (buffIcons[i].image.fillAmount <= 0)
                {
                    imagePool.Release(buffIcons[i].image);
                    buffIcons.Remove(buffIcons[i]);
                }
            }
        }

        private void Update()
        {
            UpdateBuffUI();
        }
        // private void FixedUpdate()
        // {
        //     if (FPC.isWalking)
        //     {
        //         float lerpTime = lerpSpeed;
        //         Time.timeScale = Mathf.Lerp(Time.timeScale, slowdownRatio, lerpTime);
        //     }
        //     else
        //     {
        //         Time.timeScale = 1f;
        //     }
        // }
    }
}
