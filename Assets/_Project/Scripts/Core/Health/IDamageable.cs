using UnityEngine;

namespace _Project.Scripts.Core.Health
{
    // Implémentée par tout ce qui peut prendre des dégâts (ennemis, murs, tour centrale)
    public interface IDamageable
    {
        Transform transform { get; }
        bool IsAlive { get; }
        void TakeDamage(float damage);
    }
}