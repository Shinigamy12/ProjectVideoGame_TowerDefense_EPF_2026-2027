using _Project.Scripts.Core.Weapons;

namespace _Project.Scripts.Cards
{
    // Cibles possibles des effets des cartes. Un asset ne peut pas référencer un objet de la scène :
    // le système de cartes construit ce contexte et le passe à la carte choisie.
    public class CardContext
    {
        public IWeaponInventory WeaponInventory { get; }

        public CardContext(IWeaponInventory weaponInventory)
        {
            WeaponInventory = weaponInventory;
        }
    }
}
