using System;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    [Serializable]
    public class BlockedZone
    {
        [Tooltip("Coordonner de la case bas-gauche de la zone bloquer" +
                 " (collone, ligne)")]
        [SerializeField] private Vector2Int _position;
        
        [Tooltip("Taille de la zone bloquer en nombres de cases")]
        [SerializeField] private Vector2Int _size;
        
        [Tooltip("NotBuildable : pas de défense, mais les ennemis passent. " +
                 "Impassable : ni défense ni ennemi au sol")]
        [SerializeField] private BlockedZoneType _type;

        
        public Vector2Int Position => _position;
        public Vector2Int Size => _size;
        public BlockedZoneType Type => _type;
    }
}