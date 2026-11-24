using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Core.Placement;

namespace _Project.Scripts.Grid
{
    public class GridComponent : MonoBehaviour
    {
        [Header("Dimensions")]
        [Tooltip("Nombre de cases sur l'axe X, bordure cachée comprise")]
        [SerializeField, Range(10,200)] private int _width = 92;
        [Tooltip("Nombre de cases sur l'axe Z, bordure cachée comprise")]
        [SerializeField, Range(10, 200)] private int _height = 60;
        
        [Header("Zones bloquées")]
        [Tooltip("Zones du décor où l'on ne peut pas construire, ou que " +
                 "les ennemis au sol ne peuvent pas traverser")]
        [SerializeField] private List<BlockedZone> _blockedZones = new();
        
        private bool[,] _buildable;
        private bool[,] _walkable;
        private IGridOccupant[,] _occupants; //Null = case libre
        private readonly Dictionary<IGridOccupant, List<Vector2Int>> _occupiedCells = new();

        /// <summary>
        /// Au démarrage du script, remplis les variables _buildable et _walkable de true et laisse
        /// _occupants remplie de Null. 
        /// </summary>
        private void Awake()
        {
            _buildable = new bool[_width, _height];
            _walkable = new bool[_width, _height];
            _occupants = new IGridOccupant[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    _buildable[x, z] = true;
                    _walkable[x, z] = true;
                }
            }
            ApplyBlockedZones();
        }

        /// <summary>
        /// Dessine la grille et les zones bloquées dans la vue Scene
        /// (éditeur uniquement).
        /// </summary>
        private void OnDrawGizmos()
        {
            Vector3 center = transform.position;
            Vector3 size = new Vector3(_width, 0f, _height);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = Color.grey;
            for (int i = 0; i <= _width; i++)
            {
                float x = i - _width / 2;
                Vector3 start = transform.position + new Vector3(x, 0f, -_height / 2);
                Vector3 end = transform.position + new Vector3(x, 0f, +_height / 2);
                Gizmos.DrawLine(start, end);
            }
            for (int  i = 0;  i <= _height;  i++)
            {
                float z = i - _height / 2;
                Vector3 start = transform.position + new Vector3(-_width/2, 0f, z);
                Vector3 end = transform.position + new Vector3(_width / 2, 0f, z);
                Gizmos.DrawLine(start, end);
            }

            foreach (BlockedZone zone in _blockedZones)
            {
                if (zone.Type == BlockedZoneType.Impassable)
                {
                    Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
                }
                else
                {
                    Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
                }
                Vector3 zoneCenter = CellToWorld(zone.Position)
                                     + new Vector3((zone.Size.x - 1) / 2f, 
                                         0.02f, (zone.Size.y - 1) / 2f);
                Vector3 zoneSize = new Vector3(zone.Size.x, 0f, zone.Size.y);
                Gizmos.DrawCube(zoneCenter, zoneSize);
            }
        }

        /// <summary>
        /// Donne la cellule en fonction d'une coordonnée dans le monde.
        /// </summary>
        /// <param name="worldPosition">Position en x,y,z dans le monde</param>
        /// <returns>La position de la céllule dans les tableaux en 2D</returns>
        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            Vector3 localPosition = worldPosition - transform.position;
            int x = Mathf.FloorToInt(localPosition.x) + _width / 2;
            int z = Mathf.FloorToInt(localPosition.z) + _height / 2;
            return new Vector2Int(x, z);
        }

        /// <summary>
        /// L'inverse de WordToCell, donne la position d'une céllule
        /// en position dans le monde.
        /// </summary>
        /// <param name="cell">Case (colonne, ligne)</param>
        /// <returns>Centre de la case dans le monde</returns>
        public Vector3 CellToWorld(Vector2Int cell)
        {
            float x = cell.x - _width / 2 + 0.5f;
            float z = cell.y - _height / 2 + 0.5f;
            return transform.position + new Vector3(x, 0f, z);
        }
        
        private void ApplyBlockedZones()
        {
            foreach (BlockedZone zone in _blockedZones)
            {
                for (int x = zone.Position.x; x < zone.Position.x + zone.Size.x; x++)
                {
                    for (int z = zone.Position.y; z < zone.Size.y + zone.Position.y; z++)
                    {
                        Vector2Int cell = new Vector2Int(x, z);
                        if (!IsInside(cell))
                        {
                            continue;
                        }
                        _buildable[x, z] = false;
                        if (zone.Type == BlockedZoneType.Impassable)
                        {
                            _walkable[x, z] = false;
                        }
                    }
                }
            }
        }
        

        public bool IsInside(Vector2Int cell)
        {
            return cell.x >= 0 && cell.x < _width && cell.y >= 0 && cell.y < _height;
        }

        public bool IsBuildable(Vector2Int cell)
        {
            return IsInside(cell) && _buildable[cell.x, cell.y] && 
                   _occupants[cell.x, cell.y] == null;
        }

        public bool IsWalkable(Vector2Int cell)
        {
            return IsInside(cell) && _walkable[cell.x, cell.y] && 
                   (_occupants[cell.x, cell.y] == null || 
                    !_occupants[cell.x, cell.y].IsBlockingPath);
        }
        public bool IsWalkable(Vector3 worldPosition)
        {
            return IsWalkable(WorldToCell(worldPosition));
        }
        
        [ContextMenu("Tester les conversions")]
        private void TestConversions()
        {
            Debug.Log(WorldToCell(new Vector3(0.3f, 0f, 0.3f)));
            Debug.Log(WorldToCell(new Vector3(-46f, 0f, -30f)));
            Debug.Log(CellToWorld(new Vector2Int(46, 30)));
            Debug.Log(IsInside(new Vector2Int(0,0)));
            Debug.Log(IsInside(new Vector2Int(91,59)));
            Debug.Log(IsInside(new Vector2Int(92,0)));
            Debug.Log(IsInside(new Vector2Int(-1,59)));
        }
        
        [ContextMenu("Tester les zones")]
        private void TestZones()
        {
            Vector2Int cell = new Vector2Int(11, 11);
            Debug.Log($"Case {cell} : constructible = {IsBuildable(cell)}, franchissable = {IsWalkable(cell)}");
        }
        
    }
}
