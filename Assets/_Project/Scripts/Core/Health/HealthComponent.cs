using System;
using UnityEngine;

namespace _Project.Scripts.Core.Health
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [Header("Réglages")]
        [Tooltip("PV au début de la partie")]
        [SerializeField, Min(1f)] private float _maxHealth = 10f;

        private float _currentHealth;

        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public bool IsAlive => _currentHealth > 0;

        public event Action<float, float> DamageTaken;
        public event Action<HealthComponent> Died;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive || damage <= 0f) return;
            
            _currentHealth = Mathf.Max(0f, _currentHealth - damage);
            DamageTaken?.Invoke(damage, _currentHealth);
            if (!IsAlive)
            {
                Died?.Invoke(this);
            }
        }

        public void SetMaxHealth(float maxHealth)
        {
            if (maxHealth < 1f) return;
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
        }
    }
}
