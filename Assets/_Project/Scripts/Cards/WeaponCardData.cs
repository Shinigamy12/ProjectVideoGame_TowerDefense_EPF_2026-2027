using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Cards
{
    [CreateAssetMenu(fileName = "Card_Weapon_", menuName = "Cards/Weapon Card")]
    public class WeaponCardData : CardData
    {
        [Header("Arme")]
        [SerializeField] private WeaponType _weaponType;

        public WeaponType WeaponType => _weaponType;

        public override void Apply(CardContext context)
        {
            // TODO SCRUM-124 : donner l'arme au personnage
            Debug.LogWarning($"Effet de la carte {DisplayName} pas encore branché (SCRUM-124)", this);
        }
    }
}
