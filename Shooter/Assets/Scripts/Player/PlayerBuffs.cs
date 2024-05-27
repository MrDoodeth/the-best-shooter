using System;
using System.Collections;
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

        private FirstPersonController FPC;
        private PlayerHealth playerHealth;
        private BuffImagePool imagePool;
        private List<BuffIconUI> buffIcons;

        public static Action<int> OnHealBuffActivated;
        public static Action<int> OnAmmoBuffActivated;
        public static Action<BuffState, Sprite, float, int> OnFireBuffActivated;
        public static Action<BuffState, Sprite, float, float, float> OnSlowdownBuffActivated;
        public static Action<BuffState, Sprite, float, float> OnDamageBoostBuffActivated;

        //Slowdown
        private float slowdownRatio;
        private float slowdownLerpSpeed;
        private float slowdownDuration;
        private float defaultFixedDeltaTime;
        
        //DamageBoost
        private static float damageBoostRatio = 1f;
        private static float damageBoostDuration;

        private void Awake()
        {
            FPC = GetComponent<FirstPersonController>();
            playerHealth = GetComponent<PlayerHealth>();

            buffIcons = new List<BuffIconUI>();
            
            imagePool = new BuffImagePool(imagePrefab, imageCount, buffStatusBarTransform);

            defaultFixedDeltaTime = Time.fixedDeltaTime;
        }
        private void OnEnable()
        {
            OnHealBuffActivated += OnHealBuff;
            OnAmmoBuffActivated += OnAmmoBuff;
            OnFireBuffActivated += OnFireBuff;
            OnSlowdownBuffActivated += OnSlowdownBuff;
            OnDamageBoostBuffActivated += OnDamageBoostBuff;
        }
        private void OnDisable()
        {
            OnHealBuffActivated -= OnHealBuff;
            OnAmmoBuffActivated -= OnAmmoBuff;
            OnFireBuffActivated -= OnFireBuff;
            OnSlowdownBuffActivated -= OnSlowdownBuff;
            OnDamageBoostBuffActivated -= OnDamageBoostBuff;
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

        private void OnSlowdownBuff(BuffState buffState, Sprite sprite, float duration, float slowdownRatio, float lerpSpeed)
        {
            AddBuff(buffState, sprite, duration);
            
            this.slowdownRatio = slowdownRatio;
            this.slowdownLerpSpeed = lerpSpeed;
            this.slowdownDuration = duration;
        }

        private void OnDamageBoostBuff(BuffState buffState, Sprite sprite, float duration, float damageRatio)
        {
            AddBuff(buffState, sprite, duration);

            damageBoostRatio = damageRatio;
            damageBoostDuration = duration;
        }

        public static int CalculateDamage(int damage)
        {
            if (damageBoostDuration > 0)
                return (int)(damage * damageBoostRatio);
            
            return damage;
        }

        private static void DamageBoostUpdate() => damageBoostDuration -= Time.deltaTime;

        private void SlowdownUpdate()
        {
            if (FPC.isWalking && slowdownDuration > 0)
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, slowdownRatio, slowdownLerpSpeed);
            }
            else
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, slowdownLerpSpeed);;
            }
            slowdownDuration -= Time.deltaTime;
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        }
        private void Update()
        {
            SlowdownUpdate();
            DamageBoostUpdate();
            
            UpdateBuffUI();
        }
    }
}
