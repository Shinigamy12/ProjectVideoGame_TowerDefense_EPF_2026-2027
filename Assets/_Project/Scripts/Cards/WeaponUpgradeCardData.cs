using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Cards
{
    [CreateAssetMenu(fileName = "Card_Upgrade_", menuName = "Cards/Weapon Upgrade Card")]
    public class WeaponUpgradeCardData : CardData
    {
        [Header("Amélioration")]
        [Tooltip("Arme améliorée. La carte ne doit être proposée que si le joueur possède cette arme")]
        [SerializeField] private WeaponType _weaponType;

        [SerializeField] private WeaponStat _stat;

        [Tooltip("0.1 = +10 % de la valeur de base. Pour ProjectileCount, nombre de projectiles ajoutés")]
        [SerializeField, Range(0f, 5f)] private float _amount = 0.1f;

        [SerializeField] private CardRarity _rarity;

        public WeaponType WeaponType => _weaponType;
        public WeaponStat Stat => _stat;
        public float Amount => _amount;
        public CardRarity Rarity => _rarity;

        public override void Apply(CardContext context)
        {
            // TODO SCRUM-161 (bonus) : améliorer l'arme
            Debug.LogWarning($"Effet de la carte {DisplayName} pas encore branché (SCRUM-161)", this);
        }
    }
}
