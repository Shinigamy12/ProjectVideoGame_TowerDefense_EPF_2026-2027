using _Project.Scripts.Core.Health;
using UnityEngine;

namespace _Project.Scripts.Core.Tower
{
    // Tour centrale à défendre : sa destruction fait perdre la partie
    [RequireComponent(typeof(HealthComponent))]
    public class CentralTower : MonoBehaviour
    {
        private HealthComponent _health;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
        }

        private void OnEnable() => _health.Died += OnDied;
        private void OnDisable() => _health.Died -= OnDied;

        private void OnDied(HealthComponent health)
        {
            if (GameManager.Instance == null) return;

            GameManager.Instance.TriggerDefeat();
        }
    }
}
