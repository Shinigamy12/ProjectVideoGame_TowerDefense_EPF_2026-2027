using System;
using UnityEngine;

namespace _Project.Scripts.Core.Placement
{   
    // Implémentée par le système de pose de la grille, appelée par la liste des cartes de défense
    public interface IBuildingPlacer
    {
        bool IsBuilding { get; }
        
        // Affiche la grille et la preview du prefab reçu, puis attend le clic sur une case. Ne fonctionne pas en dehors de la phase de pose
        void StartBuilding(GameObject defensePrefab);
        // Annule la pose en cours, cache la grille et la preview, sans rien poser, puis déclenche l’event d’annulation
        void CancelBuilding();
        
        // Déclenché quand la défense est posée sur une case valide. Action : la défense posée.
        event Action<GameObject> DefenseBuilt;
        // Déclenché quand la pose est annulée, que ce soit par CancelBuilding, par Échap ou par un changement de phase.
        event Action BuildCancelled;
    }
}
