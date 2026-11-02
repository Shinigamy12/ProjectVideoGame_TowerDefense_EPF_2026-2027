using UnityEngine;

namespace _Project.Scripts.Core.Health
{
    // Exemple et test du composant PV (SCRUM-171) : clic droit sur le composant dans l'inspector pour lancer les tests.
    public class HealthTester : MonoBehaviour
    {
        [Header("Réglages")]
        [Tooltip("Composant PV à tester")]
        [SerializeField] private HealthComponent _health;
        [Tooltip("Dégâts infligés à chaque test")]
        [SerializeField, Min(0f)] private float _damage = 10f;
        [Tooltip("PV max appliqués par le test SetMaxHealth")]
        [SerializeField, Min(1f)] private float _newMaxHealth = 50f;

        private void OnEnable()
        {
            _health.DamageTaken += OnDamageTaken;
            _health.Died += OnDied;
        }

        private void OnDisable()
        {
            _health.DamageTaken -= OnDamageTaken;
            _health.Died -= OnDied;
        }

        [ContextMenu("Infliger des dégâts")]
        private void DealDamage()
        {
            Debug.Log($"TakeDamage({_damage}) — avant : {_health.CurrentHealth}/{_health.MaxHealth}");
            _health.TakeDamage(_damage);
        }

        [ContextMenu("Infliger des dégâts négatifs (doit être ignoré)")]
        private void DealNegativeDamage()
        {
            _health.TakeDamage(-_damage);
            Debug.Log($"TakeDamage({-_damage}) — PV : {_health.CurrentHealth}/{_health.MaxHealth}");
        }

        [ContextMenu("Appliquer de nouveaux PV max")]
        private void ApplyMaxHealth()
        {
            _health.SetMaxHealth(_newMaxHealth);
            Debug.Log($"SetMaxHealth({_newMaxHealth}) — PV : {_health.CurrentHealth}/{_health.MaxHealth}");
        }

        private void OnDamageTaken(float damage, float remainingHealth)
        {
            Debug.Log($"DamageTaken : {damage} dégâts, {remainingHealth} PV restants");
        }

        private void OnDied(HealthComponent health)
        {
            Debug.Log($"Died : {health.name} est mort");
        }
    }
}
