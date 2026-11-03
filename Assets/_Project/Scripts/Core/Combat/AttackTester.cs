using _Project.Scripts.Core.Health;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    // Exemple et test du composant Attaque (SCRUM-173) : clic droit sur le composant dans l'inspector pour lancer les tests.
    // À lancer en mode Play : les PV des cibles ne sont remplis qu'au Awake.
    // La case « Attaque automatique » reproduit le fonctionnement d'un canon : IsReady, puis Ciblage, puis TryAttack et DealDamage.
    public class AttackTester : MonoBehaviour
    {
        [Header("Réglages")]
        [Tooltip("Composant Attaque à tester")]
        [SerializeField] private AttackComponent _attack;
        [Tooltip("Composant Ciblage utilisé pour trouver la cible")]
        [SerializeField] private TargetingComponent _targeting;
        [Tooltip("Attaque toute seule dès que le délai est écoulé, comme un canon")]
        [SerializeField] private bool _isAutoAttacking;

        private IDamageable _lastTarget;

        private void OnEnable() => _attack.Attacked += OnAttacked;
        private void OnDisable() => _attack.Attacked -= OnAttacked;

        private void Update()
        {
            if (!_isAutoAttacking || !_attack.IsReady) return;

            // On cherche la cible avant TryAttack : sans cible, le délai n'est pas consommé
            var target = _targeting.FindClosestTarget();
            if (target == null) return;

            if (_attack.TryAttack())
            {
                Hit(target);
            }
        }

        [ContextMenu("Attaquer la cible la plus proche")]
        private void AttackClosest()
        {
            if (!_attack.IsReady)
            {
                Debug.Log("Attaque refusée : le délai n'est pas écoulé");
                return;
            }

            var target = _targeting.FindClosestTarget();
            if (target == null)
            {
                Debug.Log("Aucune cible à portée : pas d'attaque, le délai n'est pas consommé");
                return;
            }

            if (_attack.TryAttack())
            {
                Hit(target);
            }
        }

        [ContextMenu("TryAttack deux fois de suite (le 2e doit être refusé)")]
        private void TryAttackTwice()
        {
            Debug.Log($"1er TryAttack : {_attack.TryAttack()}");
            Debug.Log($"2e TryAttack : {_attack.TryAttack()} (attendu : False)");
        }

        [ContextMenu("Frapper la dernière cible, même morte (doit être ignoré si morte)")]
        private void HitLastTarget()
        {
            if (_lastTarget == null)
            {
                Debug.Log("Aucune cible frappée pour l'instant");
                return;
            }

            Hit(_lastTarget);
        }

        // Test uniquement : en jeu, la pause passe par GameManager.SetPaused
        [ContextMenu("Pause / reprise (Time.timeScale)")]
        private void TogglePause()
        {
            Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
            Debug.Log($"Time.timeScale = {Time.timeScale}");
        }

        private void Hit(IDamageable target)
        {
            _lastTarget = target;
            if (!target.IsAlive)
            {
                _attack.DealDamage(target);
                Debug.Log($"{target.transform.name} est mort : DealDamage l'a ignoré");
                return;
            }

            _attack.DealDamage(target);

            var health = target as HealthComponent;
            var remaining = health != null ? health.CurrentHealth.ToString() : "?";
            Debug.Log($"{_attack.Damage} dégâts sur {target.transform.name}, PV restants : {remaining}");
            Debug.DrawLine(_attack.transform.position, target.transform.position, Color.red, 0.5f);
        }

        private void OnAttacked()
        {
            Debug.Log($"Attacked (prochaine attaque dans {_attack.Cooldown} s)");
        }
    }
}
