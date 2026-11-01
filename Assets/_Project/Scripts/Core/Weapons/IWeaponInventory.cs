namespace _Project.Scripts.Core.Weapons
{   
    // Implémentée par l’inventaire d’armes, appelée par les cartes
    public interface IWeaponInventory
    {
        bool HasWeapon(WeaponType weapon);
        
        // Ajoute l’arme à l’inventaire du joueur
        void AddWeapon(WeaponType weapon);
        // Améliore une stat de l’arme
        void UpgradeWeapon(
            WeaponType weapon,
            WeaponStat stat,
            float amount); // exemple: 0.1 =  10% d'augmentation ou 1 pour le nombre de projectiles
    }
}